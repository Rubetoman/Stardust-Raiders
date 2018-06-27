using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for bullets that will seek Player's position. Inherits from BulletController.
/// </summary>
public class SeekingBullet : BulletController {

    [Header("Seeking Configuration")]
    public float speed = 1.0f;          // Speed for the bullet advance.
    public float turnSpeed = 30.0f;     // Speed for the rotation of the bullet towards Player's position.
    public float startSeeking = 1.0f;   // Time for the bullet to start seeking.
    public float stopSeeking = 4.0f;    // Time for the bullet to stop seeking.

    private GameObject player;          // GameObject of the Player.
    private Quaternion newRotation;     // Variable used to store the new rotation to apply to the bullet.
    private float t = 0.0f;

	void Start () {
        player = GameManager.Instance.player;  // Set player variable.
    }
	
	void Update () {
        t += Time.deltaTime;
        if(t > startSeeking && t < stopSeeking)
        {
            // Find player position.
            if (player != null)
                newRotation = Quaternion.LookRotation(player.transform.position - transform.position);

            // Rotate bullet towards Player's position.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        }       
        // GoForward.
        transform.position += transform.forward * speed * Time.deltaTime;
	}
}
