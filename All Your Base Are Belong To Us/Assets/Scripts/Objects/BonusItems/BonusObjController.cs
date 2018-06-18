using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusObjController : MonoBehaviour {
    public enum RotationType
    {
        forward,
        up,
        both,
    }

    [Header("Rotation")]
    public RotationType rotationType;
    public float rotationSpeed = 10.0f;
    [Space(10)]
    [Header("Flick Effect")]
    public GameObject[] flickers;
    public Material[] materials;
    public float flickFrequency = 1.0f;
    [Space(10)]
    [Header("Score")]
    public int points = 500;

    private float timer = 0.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Rotate();
        ChangeMaterial();

    }

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
            if(GameManager.Instance != null)
                GameManager.Instance.AddToTotalScore(points);
            if(AudioManager.Instance != null)
                //if(!AudioManager.Instance.IsClipPlaying("Bonus"))
                    AudioManager.Instance.Play("Bonus");
            transform.parent = other.gameObject.transform;
            StartCoroutine("DestroyAnimation");
            Destroy(gameObject, 2);
        }
    }

    protected virtual IEnumerator DestroyAnimation()
    {
        float t = 0.0f;
        var currentPosition = transform.position;
        var currentScale = transform.localScale;
        while (t < 3)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPosition, transform.parent.position, t / 0.5f);
            transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, t / 1f);
            yield return null;
        }
    }
}
