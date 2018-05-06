using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : EnemyController {
    [Header("Tank Variables")]
    public float missileDelay = 1.0f;
    public float targetDistance = 300.0f;            // Distance at which the enemy will start shooting

    // Use this for initialization
    new void Start () {
        base.Start();
        Invoke("ShootSpawnableAmmo", missileDelay);
    }
	
	// Update is called once per frame
	void Update () {
        LookAtPlayer();
    }

    protected override void ShootSpawnableAmmo()
    {
        if (Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= targetDistance)
        {
            base.ShootSpawnableAmmo();
        }


        if (missileSpawnPoints.Length > 0)
            Invoke("ShootSpawnableAmmo", missileDelay); 
    }
}
