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

    // Use this for initialization
    protected void Start () {
        mesh = gameObject.GetComponent<MeshRenderer>();
        if (godMode)
            invulnerable = true;
    }
	
    public virtual void TakeDamage(int amount)
    {
        if (invulnerable)
            return;
        if (amount < 0)
        {
            Debug.LogError("Negative numbers not allowed");
            return;
        }
        invulnerable = true;
        currentShield -= amount;
        if (currentShield <= 0)
        {
            currentShield = 0;
            Die();
        }
        if (gameObject.activeInHierarchy && mesh != null)
            StartCoroutine(FlickeringColor(hitColor));
        else
            invulnerable = false;

    }

    protected virtual void Die()
    {
        Destroy(Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity), 1.0f);
    }

    /// <summary>
    /// This Enumerator makes the ship flick the times specified by flickCount between the normal color and hitColor
    /// </summary>
    protected IEnumerator FlickeringColor(Color newColor)
    {
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
    }
}
