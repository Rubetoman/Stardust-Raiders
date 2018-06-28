using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for controlling the health of each Boss module. Inherits from ShieldManager.
/// </summary>
public class BossModuleHealthManager : ShieldManager {

    public GameObject boss; // Boss GameObject
    public int destroyScore = 200; // Score that player wins if destroys the boss module health.

    new void Start () {
        base.Start();
    }

    /// <summary>
    /// Overriden function, inflicts damage also to the Boss Shield Manager.
    /// </summary>
    public override void TakeDamage(int amount)
    {
        if (boss == null)
        {
            Debug.LogWarning("Boss GameObject is null, only the module: " + gameObject.name + " will take damage.");
            return;
        }

        var shield = boss.GetComponent<BossShieldManager>();// Look for the Boss shield script.         
        if (shield != null)                                 // Check if the Boss shield was found.
        {
            if (!shield.invulnerable && !invulnerable)                       // Only inflict damage when is vulnerable.
            {
                shield.TakeDamage(amount);
                base.TakeDamage(amount);
            }
        }           
        else
        {
            Debug.LogWarning("The shield of " + boss.name + " is null, only the module: " + gameObject.name + " will take damage.");
        }
    }

    /// <summary>
    /// Overriden function. Destroys the gameObject and adds score to player.
    /// </summary>
    protected override void Die()
    {
        base.Die();
        GameManager.Instance.AddToTotalScore(destroyScore);
        Destroy(gameObject);
    }
}
