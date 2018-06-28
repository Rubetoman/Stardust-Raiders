using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shield manager for common enemies. Inherits from ShieldManager.
/// </summary>
public class EnemyShieldManager : ShieldManager {

    public int destroyScore = 200; // Score that player wins after defeating the enemy.

    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Destroys the GameObject and adds score to player.
    /// If it has a bullets as a child they will be unparented so they are not destroyed.
    /// </summary>
    protected override void Die()
    {
        base.Die();
        foreach (Transform child in transform)  // Look for child
        {
            if(child.CompareTag("Bullet"))      // Filter bullets
                child.parent = null;
        }
        Destroy(gameObject);
        GameManager.Instance.AddToTotalScore(destroyScore);
    }
}
