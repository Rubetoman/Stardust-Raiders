using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damageToPlayer = 10;
    public int damageToEnemy = 10;

    protected void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        if (other.CompareTag("Enemy"))
            DamageEnemy(hit);
        else if (other.CompareTag("PlayerCollider"))
            DamagePlayer(hit);

        Destroy(gameObject);
    }

    protected void DamagePlayer(GameObject player)
    {
        var shield = player.GetComponent<PlayerShieldManager>();
        if (shield != null && !shield.invulnerable)
        {
            shield.TakeDamage(damageToPlayer);
        }
    }

    protected void DamageEnemy(GameObject enemy)
    {
        var shield = enemy.GetComponent<EnemyShieldManager>();
        if (shield != null && !shield.invulnerable)
        {
            shield.TakeDamage(damageToEnemy);
        }
    }
}
