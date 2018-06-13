using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the proximity mines. The nearest the player is to the mine the fastest it will move towards the player. It explodes with any collision.
/// </summary>
public class MineController : MonoBehaviour {

    public int damage = 25;                 // Damage the player will take when colliding.
    public float targetDistance = 100.0f;   // Distance at which it will start moving forward the player.
    public GameObject explosionEffect;      // Player transform.
    private Transform player;

	void Start () {
        // Get player Transform.
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void FixedUpdate () {
        // Calculate distance to the player.
        var distanceToPlayer = (player.transform.position - transform.position).magnitude;

        if (distanceToPlayer <= targetDistance)  // If its near enought start moving towards the player.
            transform.position = Vector3.Lerp(transform.position, player.position, (targetDistance / distanceToPlayer) * Time.deltaTime * 0.7f);
    }

    /// <summary>
    /// Explosion effect: it will leave a explosion and destroy himself.
    /// </summary>
    void Explode()
    {
        Destroy(Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity), 1.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerCollider"))                              // If the collision was with the player, the player shield will take the damage
            other.GetComponent<PlayerShieldManager>().TakeDamage(damage);

        Explode();
    }
}
