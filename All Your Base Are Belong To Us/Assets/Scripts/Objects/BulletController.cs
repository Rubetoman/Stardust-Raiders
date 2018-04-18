using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public int damageToPlayer = 10;
    public int damageToEnemy = 10;

    private void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        if (other.CompareTag("Enemy"))
            DamageEnemy(hit);
        else if (other.CompareTag("Player"))
            DamagePlayer(hit);

        Destroy(gameObject);
    }

    private void DamagePlayer(GameObject player)
    {
        var shield = player.GetComponent<PlayerShieldManager>();
        if (shield != null && !shield.damaged)
        {
            shield.TakeDamage(damageToPlayer);
        }
    }

    private void DamageEnemy(GameObject enemy)
    {
        var shield = enemy.GetComponent<EnemyShieldManager>();
        if (shield != null && !shield.damaged)
        {
            shield.TakeDamage(damageToEnemy);
        }
    }
}
