using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldManager : ShieldManager {

    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Destroys the GameObject
    /// If it has a parent it will destroy the whole GameObject hierarchy if not only the objetc the script is attached to.
    /// </summary>
    protected override void Die()
    {
        base.Die();
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);
        else
            Destroy(gameObject);
    }
}
