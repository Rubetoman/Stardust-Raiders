using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModuleHealthManager : ShieldManager {
    public string collisionTag = "Bullet";
    public int hitDamage = 10;
    public GameObject boss;

    // Use this for initialization
    new void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
		
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


    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

}
