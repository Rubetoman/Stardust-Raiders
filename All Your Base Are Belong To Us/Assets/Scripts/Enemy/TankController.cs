using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll enemy tanks behaviour. It lets them shoot while the player is near.
/// Inherits form EnemyController.
/// </summary>
public class TankController : EnemyController {
    [Header("Tank Variables")]
    public float shootDelay = 1.0f;         // Delay between bullet spawn. 
    public float targetDistance = 300.0f;   // Distance at which the enemy will start shooting.


    new void Start () {
        base.Start();
        Invoke("ShootSpawnableAmmo", shootDelay);   // Start shooting.
    }
	

	void Update () {
        if (transform != null)
            LookAtPlayer();
    }

    /// <summary>
    /// Overriden function to shoot bullets, they are only spawned if the player distance is lower than targetDistance.
    /// </summary>
    protected override void ShootSpawnableAmmo()
    {
        if (Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= targetDistance)
            base.ShootSpawnableAmmo();

        if (bulletSpawnPoints.Length > 0)   // Avoid to keep shooting if there are no spawn points.
            Invoke("ShootSpawnableAmmo", shootDelay);
        else
            Debug.Log(gameObject.name + ": spawn points could't be found.");
    }
}
