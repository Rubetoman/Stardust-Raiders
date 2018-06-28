using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll every obstacle. It lets them to make damage to the Player.
/// </summary>
public class ObstacleController : MonoBehaviour {

    public int damage = 10;     // Damage that the Player will receive upon collision.

    private void OnTriggerEnter(Collider other)
    {
        var shield = other.gameObject.GetComponent<PlayerShieldManager>();  // Get Player's shield script.
        if (shield != null)
            shield.TakeDamage(damage);                                      // Apply damage.
    }

    private void OnTriggerStay(Collider other)
    {
        var shield = other.gameObject.GetComponent<PlayerShieldManager>();  // Get Player's shield script.
        if (shield != null)
            shield.TakeDamage(damage);                                      // Apply damage.
    }
}
