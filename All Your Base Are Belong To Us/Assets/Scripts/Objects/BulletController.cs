using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damageToPlayer = 10;
    public int damageToEnemy = 10;
    public int damageToObstacle = 10;

    protected void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        switch (other.tag)
        {
            case "Enemy":
                DamageEnemy(hit);
                break;
            case "PlayerCollider":
                DamagePlayer(hit);
                break;
            case "Destructible":
                DamageObstacle(hit);
                break;
        }

        Destroy(gameObject);
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
}
