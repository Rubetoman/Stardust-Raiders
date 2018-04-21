using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldManager : ShieldManager {

    new void Start()
    {
        base.Start();
    }


    protected override void Die()
    {
        base.Die();
        Destroy(transform.parent);
    }
}
