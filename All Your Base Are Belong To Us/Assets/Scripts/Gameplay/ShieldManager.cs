using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour {
    [Header("Shield")]
    public const int maxShield = 100;
    public int currentShield = maxShield;
    public bool godMode = false;
    [Space(10)]
    [Header("Hit Effect")]
    public float recoverTime = 2f;
    public Color hitColor;
    public int flickCount = 5;
    public float flickRate = 0.1f;
    public GameObject deathEffect;

    protected MeshRenderer mesh;
    protected Color normalColor;

    [HideInInspector]
    public bool invulnerable = false;
    private bool flickeringColor = false;
    // Use this for initialization
    protected void Start () {
        mesh = gameObject.GetComponent<MeshRenderer>();
        if (godMode)
            invulnerable = true;
    }
	
    /// <summary>
    /// Function that manages the damage taken by the shield.
    /// </summary>
    /// <param name="amount"> Amount of damage taken.</param>
    public virtual void TakeDamage(int amount)
    {
        if (invulnerable)
            return;
        if (amount < 0)
        {
            Debug.LogError("Negative numbers not allowed");
            return;
        }
        currentShield -= amount;
        if (currentShield <= 0)
        {
            currentShield = 0;
            Die();
        }
        if (gameObject.activeInHierarchy && mesh != null && recoverTime > 0)    // Check if the gameObject containing the script still active and has recover time
            StartCoroutine(HitEffect());
    }

    protected virtual void Die()
    {
        Destroy(Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity), 1.0f);
    }

    /// <summary>
    /// Function that controlls the actions that will take place after the shield gets hit.
    /// </summary>
    protected IEnumerator HitEffect()
    {
        //yield return new WaitForSeconds(0.1f);  // Delay so if the shield gets multiple hits at the same time all of them damage
        invulnerable = true;                    // Make 
        if(!flickeringColor)
            StartCoroutine(FlickeringColor(hitColor));

        yield return null;
    }

    /// <summary>
    /// This Enumerator makes the ship flick the times specified by flickCount between the normal color and hitColor
    /// </summary>
    protected IEnumerator FlickeringColor(Color newColor)
    {
        flickeringColor = true;
        normalColor = mesh.material.color;
        for (int i = 0; i <= flickCount; i++)
        {
            mesh.material.color = newColor;
            yield return new WaitForSeconds(flickRate);
            mesh.material.color = normalColor;
            yield return new WaitForSeconds(flickRate);
        }
        //Be able to be damaged again
        invulnerable = false;
        flickeringColor = false;
    }
}
