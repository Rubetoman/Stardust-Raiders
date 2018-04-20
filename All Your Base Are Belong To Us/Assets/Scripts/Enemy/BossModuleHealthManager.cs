using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModuleHealthManager : MonoBehaviour {
    public string collisionTag = "Bullet";
    public const int maxShield = 100;
    public int currentShield = maxShield;
    public Color hitColor;
    public int hitDamage = 10;
    public int flickCount = 5;
    public float flickRate = 0.1f;
    public GameObject boss;
    public GameObject explosion;

    private Color normalColor;
    private MeshRenderer mesh;
    // Use this for initialization
    void Start () {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
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
            DestroyModule();
        }
        StartCoroutine(FlickeringColor(hitColor));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == collisionTag)
        {
            TakeDamage(hitDamage);
            var shield = boss.GetComponent<BossShieldManager>();
            if(shield != null)
            {
                shield.TakeDamage(hitDamage);
            }
        }
    }

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
    }

    void DestroyModule()
    {
        Destroy(Instantiate(explosion, transform.position, Random.rotation), 2.0f);
        Destroy(gameObject);
    }

}
