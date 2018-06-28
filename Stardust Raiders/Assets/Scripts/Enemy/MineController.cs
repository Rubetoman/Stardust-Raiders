using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the proximity mines. The nearest the player is to the mine the fastest it will move towards the player. It explodes with any collision.
/// </summary>
public class MineController : MonoBehaviour {

    public int damage = 25;                 // Damage the player will take when colliding.
    public float targetDistance = 100.0f;   // Distance at which it will start moving forward the player.
    public float speed = 1;                 // Speed for the movement.
    public GameObject explosionEffect;      // Explosion to spawn when the mine is destroyed.

    private GameObject player;               // Player transform.

    void Start () {
        player = GameManager.Instance.player;  // Set player variable.
    }
	
	void FixedUpdate () {
        if (player == null)     // Avoid executing code if player variable is null.
        {
            Debug.LogWarning("Player couldn't be found. Searching again...");
            player = GameManager.Instance.player;    // Set player variable.
            if (player != null)
                Debug.LogWarning("Player found!");
            else
                return;
        }
        var distanceToPlayer = (player.transform.position - transform.position).magnitude;  // Calculate distance to the player.

        if (distanceToPlayer <= targetDistance)                                             // If its near enought start moving towards the player.
            transform.position = Vector3.Lerp(transform.position, player.transform.position, (targetDistance / distanceToPlayer) * Time.deltaTime * speed);
    }

    /// <summary>
    /// Explosion effect: it will leave a explosion and destroy himself.
    /// </summary>
    void Explode()
    {
        Destroy(Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity), 1.0f);    // Spawn explosion.
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerCollider"))  // If the collision was with the player, the player shield will take the damage
            other.GetComponent<PlayerShieldManager>().TakeDamage(damage);
        Explode();                              // Make the mine to explode.
    }
}
