using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for controlling the health of each Boss module. Inherits from ShieldManager.
/// </summary>
public class BossModuleHealthManager : ShieldManager {

    public GameObject boss; // Boss GameObject

    new void Start () {
        base.Start();
    }

    /// <summary>
    /// Overriden function, inflicts damage also to the Boss Shield Manager.
    /// </summary>
    /// <param name="amount"></param>
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (boss == null)
        {
            Debug.LogWarning("Boss GameObject is null, only the module: " + gameObject.name + " will take damage.");
            return;
        }

        var shield = boss.GetComponent<BossShieldManager>();// Look for the Boss shield script.         
        if (shield != null)                                 // Check if the Boss shield was found.
        {
            if(!shield.invulnerable)                        // Only inflict damage when is vulnerable.
                shield.TakeDamage(amount);
        }           
        else
        {
            Debug.LogWarning("The shield of " + boss.name + " is null, only the module: " + gameObject.name + " will take damage.");
        }
    }

    /// <summary>
    /// Overriden function. destroys the gameObject.
    /// </summary>
    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
