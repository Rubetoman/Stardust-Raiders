using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll enemy ship behaviour. It lets them shoot in a different way to common enemies and stop staying in front of the player.
/// Inherits form EnemyController.
/// </summary>
public class EnemyShipController : EnemyController {
    [Header("Stay In Front")]
    public float targetDistance = 10.0f;        // Distance at which the enemy will stay in front of player.
    public float forwardSpeed = 0.0f;           // Speed at which they will move forward before being near the player.
    public float leaveTime = 10.0f;             // After this time the enemy will leave the scene.
    [Space(10)]
    [Header("Shoot Variables")]
    public float shootDelay = 2.0f;             // Delay between bullet spawn.
    public float shootDistance = 300.0f;        // Distance at which the enemy will start shooting.
    public float bulletSpeed = 10.0f;           // Speed of the bullets spawned.
    public bool doubleShoot = false;            // If true enemy will spawn two bullets with every shoot.

    private bool called = false;                // Bool to call part of the LateUpdate function only once. 
    protected GameObject gameplayPlane;         // gameplayPlane GameObject where the player moves.
    protected Transform ground;                 // ground plane's Transform, used for reference of level orientation.

    new void Start () {
        base.Start();
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");  // Set gameplayPlane variable.
        ground = GameObject.FindGameObjectWithTag("Ground").transform;      // Set ground variable.
    }
	
	void LateUpdate () {
        LookAtPlayer();
        // Project distance between enemy and gamePlay plane to forward vector of the ground. The ground is used as a reference because it doesn't move during the game.
        if (Vector3.Project(gameplayPlane.transform.position - transform.position, ground.transform.forward).magnitude <= targetDistance)
        {
            // If the gameObject is moving locally and has a parent means that we want to keep the parent with us.
            if (localMovement && transform.parent != null)
                StayInFrontOf(gameplayPlane, targetDistance, transform.parent);
            else
                StayInFrontOf(gameplayPlane, targetDistance, transform);

            if (!called)    // Following code will be called just once.
            {
                MoveEnemy(transform.position, moveAxis, localMovement); // Start enemy movement.
                ShootSpawnableAmmo();                                   // Start shooting.
                if (doubleShoot)
                    Invoke("ShootSpawnableAmmo", shootDelay + 0.2f);
                Invoke("LeaveScene",leaveTime);                         // Invoke function to leave scene with the specified delay.
                called = true;
            }
            
        }
        else
        {
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;    // Move forward.
        }
	}

    /// <summary>
    /// Overriden function to shoot bullets, instead of just spawning them, a forward force is applied to them.
    /// </summary>
    protected override void ShootSpawnableAmmo()
    {
        base.ShootSpawnableAmmo();
        if(bullet != null)
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);            // Apply forward force.
        if (Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= shootDistance)   // If player is inside shoot distance, keep shooting.
            Invoke("ShootSpawnableAmmo", shootDelay);
    }

    /// <summary>
    /// Function to stop staying in front of player. A movement is made to avoid colliding with player and make it more noticeable.
    /// </summary>
    void LeaveScene()
    {
        Vector3 outScenePos = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
        MoveEnemy(outScenePos, moveAxis, localMovement);
        shootDistance = -100f;  // Stop shooting.
        targetDistance = -10f;  // Stopt staying in front of player.
    }
}
