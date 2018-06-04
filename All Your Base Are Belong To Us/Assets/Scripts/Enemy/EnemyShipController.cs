using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : EnemyController {
    [Header("Stay In Front")]
    public float targetDistance = 10.0f;
    public float forwardSpeed = 0.0f;
    public float leaveTime = 10.0f;             // After this time the enemy will leave the scene
    [Space(10)]
    [Header("Shoot Variables")]
    public float shootDelay = 2.0f;
    public float shootDistance = 300.0f;        // Distance at which the enemy will start shooting
    public float bulletVelocity = 10.0f;
    public bool doubleShoot = false;

    private bool called = false;
    protected GameObject gameplayPlane;
    protected Transform ground;
    // Use this for initialization
    new void Start () {
        base.Start();
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
        ground = GameObject.FindGameObjectWithTag("Ground").transform;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        LookAtPlayer();
        // Project distance between enemy and gamePlay plane to forward vector of the ground. The ground is used as a reference because it doesn't move during the game.
        if (Vector3.Project(gameplayPlane.transform.position - transform.position, ground.transform.forward).magnitude <= targetDistance)
        {
            StandInFrontOf(gameplayPlane, targetDistance);
            if (!called)
            {
                MoveEnemy(transform.position, moveAxis);
                ShootSpawnableAmmo();
                if (doubleShoot)
                    Invoke("ShootSpawnableAmmo", shootDelay + 0.2f);
                Invoke("LeaveScene",leaveTime);
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
        if(missile != null)
            missile.GetComponent<Rigidbody>().AddForce(missile.transform.forward * bulletVelocity, ForceMode.VelocityChange);
        if (Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= shootDistance)
        {
            Invoke("ShootSpawnableAmmo", shootDelay);
        }
    }

    void LeaveScene()
    {
        Vector3 outScenePos = new Vector3(transform.position.x, transform.position.y + 100f, transform.position.z);
        MoveEnemy(outScenePos, moveAxis);
        shootDistance = -100f;
        targetDistance = -10f;
    }
}
