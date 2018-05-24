using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModuleHealthManager : ShieldManager {
    public string collisionTag = "Bullet";
    public GameObject boss;

    // Use this for initialization
    new void Start () {
        base.Start();
    }

    public override void TakeDamage(int amount)
    {
        var shield = boss.GetComponent<BossShieldManager>();
        if (shield != null && !invulnerable)
        {
            shield.TakeDamage(amount);
        }
        base.TakeDamage(amount);
    }


    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

}
