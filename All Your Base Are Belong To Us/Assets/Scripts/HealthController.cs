using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
    public const int maxHealth = 100;
    public int currentHealth = maxHealth;
    public float recoverTime = 2f;
    public RectTransform healthBar;
    //public MeshRenderer[] meshes; 
    public MeshRenderer mesh;
    public Color hitColor;
    public int flickCount = 5;
    public float flickRate = 0.1f;

    public bool damaged = false;


    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }

        StartCoroutine(FlickeringColor());
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        StartCoroutine(RecoverTime());
    }

    IEnumerator FlickeringColor()
    {
        for(int i=0; i<=flickCount; i++)
        {
            mesh.material.color = hitColor;
            yield return new WaitForSeconds(flickRate);
            mesh.material.color = Color.white;
            yield return new WaitForSeconds(flickRate);
        }
    }

    IEnumerator RecoverTime()
    {
        yield return new WaitForSeconds(recoverTime);
        damaged = false;
    }
}
