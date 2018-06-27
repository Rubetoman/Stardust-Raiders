using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll damage of normal bullets.
/// </summary>
public class BulletController : MonoBehaviour {

    public int damageToPlayer = 10;
    public int damageToEnemy = 10;
    public int damageToObstacle = 10;
    public int damageToBoss = 10;

    protected void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        switch (other.tag)                              // Find tag of collided GameObjects.
        {
            case "Enemy":                               // Enemy: Damage enemy, destroy bullet and play sound effect.
                if (damageToEnemy > 0)
                    DamageEnemy(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "PlayerCollider":                      // Player: Damage player, destroy bullet and play sound effect.
                if (damageToPlayer > 0)
                    DamagePlayer(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "Destructible":                        // Destructible Obstacle: Damage obstacle, destroy bullet and play sound effect.
                if (damageToObstacle > 0)
                    DamageObstacle(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "Boss":                                // Boss: Damage boss, destroy bullet and play sound effect.
                if (damageToBoss > 0)
                    DamageBoss(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "BossModule":                          // Boss Module: Damage boss module, destroy bullet and play sound effect.
                if (damageToBoss > 0)
                    DamageBossModule(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "Bullet":                              // Bullet: Destroy bullet and play sound effect.
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "Obstacle":                            // Obstacle: Destroy bullet.
                Destroy(gameObject);
                break;
        }
    }

    #region Damage Shield Functions
    /// <summary>
    /// Function to find Player shield script and apply damage.
    /// </summary>
    /// <param name="player"> GameObject of the Player.</param>
    protected void DamagePlayer(GameObject player)
    {
        var shield = player.GetComponent<PlayerShieldManager>();
        if (shield != null && !shield.inBarrelRoll)                 // When in barrel roll player is invulnerable to bullets.
        {
            shield.TakeDamage(damageToPlayer);
        }
    }

    /// <summary>
    /// Function to find enemy shield script and apply damage.
    /// </summary>
    /// <param name="enemy"> GameObject of the enemy.</param>
    protected void DamageEnemy(GameObject enemy)
    {
        var shield = enemy.GetComponent<EnemyShieldManager>();
        if (shield != null)
        {
            shield.TakeDamage(damageToEnemy);
        }
    }

    /// <summary>
    /// Function to find obstacle shield script and apply damage.
    /// </summary>
    /// <param name="obstacle"> GameObject of the obstacle.</param>
    protected void DamageObstacle(GameObject obstacle)
    {
        var shield = obstacle.GetComponent<ObstacleShieldManager>();
        if (shield != null)
        {
            shield.TakeDamage(damageToObstacle);
        }
    }

    /// <summary>
    /// Function to find Boss shield script and apply damage.
    /// </summary>
    /// <param name="boss"> GameObject of the Boss.</param>
    protected void DamageBoss(GameObject boss)
    {
        var shield = boss.GetComponent<BossShieldManager>();
        if (shield != null)
        {
            shield.TakeDamage(damageToBoss);
        }
    }

    /// <summary>
    /// Function to find Boss Module shield script and apply damage.
    /// </summary>
    /// <param name="module"> GameObject of the Boss Module.</param>
    protected void DamageBossModule(GameObject module)
    {
        var shield = module.GetComponent<BossModuleHealthManager>();
        if (shield != null)
        {
            AudioManager.Instance.Play("BossHit");
            shield.TakeDamage(damageToBoss);
        }
    }

    #endregion

    #region Set Damage Functions

    /// <summary>
    /// Function that changes the damage for every option of GameObject in the script.
    /// </summary>
    /// <param name="newDamage"> New value for damage.</param>
    public void SetAllDamages(int newDamage)
    {
        damageToPlayer = newDamage;
        damageToEnemy = newDamage;
        damageToObstacle = newDamage;
        damageToBoss = newDamage;
    }

    /// <summary>
    /// Function that changes the damage for the Player.
    /// </summary>
    /// <param name="newDamage"> New value for damage.</param>
    public void SetPlayerDamage(int newDamage)
    {
        damageToPlayer = newDamage;
    }

    /// <summary>
    /// Function that changes the damage for the enemies.
    /// </summary>
    /// <param name="newDamage"> New value for damage.</param>
    public void SetEnemyDamage(int newDamage)
    {
        damageToEnemy = newDamage;
    }

    /// <summary>
    /// Function that changes the damage for the destructible obstacles.
    /// </summary>
    /// <param name="newDamage"> New value for damage.</param>
    public void SetObstacleDamage(int newDamage)
    {
        damageToObstacle = newDamage;
    }

    /// <summary>
    /// Function that changes the damage for the Boss or Boss Modules.
    /// </summary>
    /// <param name="newDamage"> New value for damage.</param>
    public void SetBossDamage(int newDamage)
    {
        damageToBoss = newDamage;
    }

    #endregion
}
