using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecoverObjController : BonusObjController
{

    public int shieldToRecover = 50;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var hit = other.gameObject;
        var shield = hit.GetComponent<PlayerShieldManager>();
        if (shield != null)
            shield.RecoverShield(shieldToRecover);
    }
}
