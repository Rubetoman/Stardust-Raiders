using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : EnemyController {
    [Header("Stay In Front")]
    public float targetDistance = 10.0f;
    public float forwardSpeed = 0.0f;
    [Space(10)]
    [Header("Shoot Variables")]
    public float shootDelay = 2.0f;
    public float shootDistance = 300.0f;            // Distance at which the enemy will start shooting
    public float bulletVelocity = 10.0f;
    public bool doubleShoot = false;

    private bool called = false;
    protected GameObject gameplayPlane;
    // Use this for initialization
    new void Start () {
        base.Start();
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        LookAtPlayer();

        if (Vector3.Project(gameplayPlane.transform.position - transform.position, gameplayPlane.transform.forward).magnitude <= targetDistance)
        {
           
            StandInFrontOf(gameplayPlane, targetDistance);
            if (!called)
            {
                MoveEnemy(transform.position, moveAxis, loopMovement);
                ShootSpawnableAmmo();
                if (doubleShoot)
                    Invoke("ShootSpawnableAmmo", shootDelay + 0.2f);
                called = true;
            }
            
        }
        else
        {
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        }
	}

    protected override void ShootSpawnableAmmo()
    {
        base.ShootSpawnableAmmo();
        missile.GetComponent<Rigidbody>().AddForce(missile.transform.forward * bulletVelocity, ForceMode.VelocityChange);
        if (Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= shootDistance)
        {
            Invoke("ShootSpawnableAmmo", shootDelay);
        }
    }

}
