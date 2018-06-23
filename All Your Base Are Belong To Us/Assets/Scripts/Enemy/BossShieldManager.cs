using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shield controller of the Boss. Inherits from ShieldManager.
/// </summary>
public class BossShieldManager : ShieldManager {

    public GameObject winScreen; // GameObject of the winning that contains the wining function to run once the Boss was destroyed. (Optional)

    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Overriden function, stops all sound effects, unparents all bullets to avoid destroying them, destoys the Boss child GameObjects and starts an animation.
    /// </summary>
    protected override void Die()
    {
        base.Die();
        AudioManager.Instance.StopSoundEffects();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Bullet")) // Unparent bullets.
                child.parent = null;
            else
                Destroy(child.gameObject);  // Destoy child.
        }
        StartCoroutine(BossDieAnimation()); // Start die aniamtion.
    }

    /// <summary>
    /// Overriden function, updates the enemy shield bar shown on the player's HUD.
    /// </summary>
    /// <param name="amount"></param>
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        PlayerHUDManager.Instance.SetEnemyShieldBarWidth(currentShield);
    }

    /// <summary>
    /// Animation for the Boss destruction. Spawns 30 explosions on a random position within the range with 0.2 seconds of delay between them.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BossDieAnimation()
    {
        for(int i = 0; i < 30; i++) 
        {
            Vector3 explosionSpawn = Random.insideUnitSphere * 2;
            explosionSpawn.z = transform.position.z;
            Destroy(Instantiate(deathEffect, new Vector3(Random.Range(-6, 6), Random.Range(-6, 6), transform.position.z), Quaternion.identity), 1f);
            yield return new WaitForSeconds(0.2f);
        }
        LevelManager.Instance.LoopSectorActive(false);              // Set sector looping to false.
        PlayerHUDManager.Instance.SetEnemyShieldBarActive(false);   // Hide the Enemy shield bar from player's HUD.
        if (winScreen != null)
            winScreen.GetComponent<WinScreen>().Win();              // Call to win function of Win Screen,
        Destroy(gameObject);                                        // Destoy the Boss GameObject.
    }
}
