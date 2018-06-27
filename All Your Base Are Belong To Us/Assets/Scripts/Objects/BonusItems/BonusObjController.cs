using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll every bonus pick-upable GameObject. It lets them to rotate, flick between materials and being picked up.
/// </summary>
public class BonusObjController : MonoBehaviour {

    // Axis around which the GameObject will rotate.
    public enum RotationType
    {
        forward,    // Forward axis taken from the Transform.
        up,         // Up axis taken from the Transform.
        both,       // Up and Forward axis taken from the Transform.
    }

    [Header("Rotation")]
    public RotationType rotationType;       // Type of rotation.
    public float rotationSpeed = 10.0f;     // Speed of the rotation.
    [Space(10)]
    [Header("Flick Effect")]
    public GameObject[] flickers;           // Parts of the GameObject that will flick.
    public Material[] materials;            // Materials to flick between.
    public float flickFrequency = 1.0f;     // Time between material change.
    [Space(10)]
    [Header("Score")]
    public int points = 500;                // Points to add to score for picking the Bonus GameObject.
    private float timer = 0.0f;             // Internal Timer.

    void Update()
    {
        Rotate();           // Keep roating.
        ChangeMaterial();   // Change materials.
    }
    /// <summary>
    /// Function to make the Bonus GameObject rotate around the selected axis.
    /// </summary>
    private void Rotate()
    {
        if (rotationType == RotationType.forward)
        {
            transform.RotateAround(transform.position, transform.forward, rotationSpeed * Time.deltaTime);
        }
        else if (rotationType == RotationType.up)
        {
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(transform.position, transform.forward, rotationSpeed * Time.deltaTime);
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Change material of the selected GameObjects for the materials given.
    /// </summary>
    private void ChangeMaterial()
    {
        foreach (Material m in materials)
        {
            timer += Time.deltaTime;
            if (timer > flickFrequency)
            {
                foreach (GameObject f in flickers)
                {
                    if (f != null)
                        f.GetComponent<Renderer>().material = m;
                }
                timer = 0.0f;
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            Destroy(gameObject.GetComponent<Collider>());       // Destroy Collider to avoid Trigger Enter again.
            if(GameManager.Instance != null)
                GameManager.Instance.AddToTotalScore(points);   // Add points to score.
            if(AudioManager.Instance != null)
                AudioManager.Instance.Play("Bonus");            // Play sound effect.
            transform.parent = other.gameObject.transform;      // Make bonus GameObject parent of Player.
            StartCoroutine("DestroyAnimation");                 // Play animation.
            Destroy(gameObject, 2);                             // Destroy after 2 seconds.
        }
    }

    /// <summary>
    /// Function that controlls the animation for the Bonus GameObject onced it is picked.
    /// </summary>
    protected virtual IEnumerator DestroyAnimation()
    {
        float t = 0.0f;
        var currentPosition = transform.position;
        var currentScale = transform.localScale;
        // Move smoothly towards Player.
        while (t < 3)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPosition, transform.parent.position, t / 0.5f);
            transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, t / 1f);
            yield return null;
        }
    }
}
