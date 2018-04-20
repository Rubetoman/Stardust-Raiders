using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldManager : MonoBehaviour {
    public const int maxShield = 500;
    public int currentShield = maxShield;
    public RectTransform bossShieldBar;
    public GameObject explosion;
    //public GameObject deathEffect;
    //public GameObject damageEffect;

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
        currentShield -= amount;

        if (currentShield <= 0)
        {
            currentShield = 0;
            DestroyBoss();
        }
        bossShieldBar.sizeDelta = new Vector2(currentShield, bossShieldBar.sizeDelta.y);
    }

    void DestroyBoss()
    {
        Destroy(Instantiate(explosion, transform.position, Random.rotation), 2.0f);
        Destroy(gameObject);
    }
}
