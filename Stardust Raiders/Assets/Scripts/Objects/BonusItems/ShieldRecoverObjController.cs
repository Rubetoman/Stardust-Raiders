using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the Bonus GameObject that recovers Player shield. Inherits from BonusObjController.
/// </summary>
public class ShieldRecoverObjController : BonusObjController
{
    public int shieldToRecover = 50;    // Amount of shield to recover.

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerCollider"))                // Only Player can pick it up.
            return;
        base.OnTriggerEnter(other);
        var hit = other.gameObject;
        var shield = hit.GetComponent<PlayerShieldManager>();   // Look fo Player's shield script.  
        if (shield != null)
            shield.RecoverShield(shieldToRecover);              // Recover shield with the amount given.
    }
}
