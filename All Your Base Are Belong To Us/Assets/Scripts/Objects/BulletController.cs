using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damageToPlayer = 10;
    public int damageToEnemy = 10;
    public int damageToObstacle = 10;
    public int damageToBoss = 10;

    protected void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        switch (other.tag)
        {
            case "Enemy":
                if (damageToEnemy > 0)
                {
                    DamageEnemy(hit);
                    Destroy(gameObject);
                    AudioManager.Instance.Play("Hit");
                }
                break;
            case "PlayerCollider":
                if(damageToPlayer > 0)
                {
                    DamagePlayer(hit);
                    Destroy(gameObject);
                    AudioManager.Instance.Play("Hit");
                }
                break;
            case "Destructible":
                if(damageToObstacle > 0)
                    DamageObstacle(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "Boss":
                if (damageToBoss > 0)
                    DamageBoss(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "BossModule":
                if (damageToBoss > 0)
                    DamageBossModule(hit);
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
            case "Bullet":
                Destroy(gameObject);
                AudioManager.Instance.Play("Hit");
                break;
        }
    }

    protected void DamagePlayer(GameObject player)
    {
        var shield = player.GetComponent<PlayerShieldManager>();
        if (shield != null && !shield.inBarrelRoll)
        {
            shield.TakeDamage(damageToPlayer);
        }
    }

    protected void DamageEnemy(GameObject enemy)
    {
        var shield = enemy.GetComponent<EnemyShieldManager>();
        if (shield != null /*&& !shield.invulnerable*/)
        {
            shield.TakeDamage(damageToEnemy);
        }
    }

    protected void DamageObstacle(GameObject obstacle)
    {
        var shield = obstacle.GetComponent<ObstacleShieldManager>();
        if (shield != null /*&& !shield.invulnerable*/)
        {
            shield.TakeDamage(damageToObstacle);
        }
    }

    protected void DamageBoss(GameObject boss)
    {
        var shield = boss.GetComponent<BossShieldManager>();
        if (shield != null /*&& !shield.invulnerable*/)
        {
            shield.TakeDamage(damageToBoss);
        }
    }

    protected void DamageBossModule(GameObject module)
    {
        var shield = module.GetComponent<BossModuleHealthManager>();
        if (shield != null /*&& !shield.invulnerable*/)
        {
            shield.TakeDamage(damageToBoss);
        }
    }
}
