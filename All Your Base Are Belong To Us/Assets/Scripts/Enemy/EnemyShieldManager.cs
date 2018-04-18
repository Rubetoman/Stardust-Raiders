using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldManager : MonoBehaviour {
    public const int maxShield = 110;
    public int currentShield = maxShield;
    public float recoverTime = 2f;
    public Color hitColor;
    public int flickCount = 5;
    public float flickRate = 0.1f;
    public bool damaged = false;
    public GameObject explosion;

    private Color normalColor;
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }
    public void TakeDamage(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Negative numbers not allowed, enemies can't recover shield");
            return;
        }
        damaged = true;
        currentShield -= amount;
        if (currentShield <= 0)
        {
            currentShield = 0;
            DestroyEnemy();
        }
        StartCoroutine(FlickeringColor(hitColor));
    }

    void DestroyEnemy()
    {
        Destroy(Instantiate(explosion, transform.position, Random.rotation), 2.0f);
        Destroy(gameObject);
    }

    /// <summary>
    /// This Enumerator makes the ship flick the times specified by flickCount between the normal color and hitColor
    /// </summary>
    IEnumerator FlickeringColor(Color newColor)
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
        damaged = false;
    }
}
