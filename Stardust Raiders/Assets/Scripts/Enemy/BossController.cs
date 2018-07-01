using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll Boss behaviour. Inherits form EnemyController
/// </summary>
public class BossController : EnemyController {
    [Header("Boss Parts")]                  // Geometry of the Boss GameObject for referencing.
    public Transform turretBase;            // The base of the turret (needs to be pointing forward).
    public GameObject frontMissileSpawner;  // GameObject where spawnable ammo will be spawned from.
    public GameObject[] thrusters;          // Thrusters that enable Boss movement.

    [Space(10)]
    [Header("Boss Variables")]
    public float spawnAmmoDelay = 1.0f;     // Delay between ammo spawn.
    [Space(10)]
    [Header("Laser Attack")]
    public Transform[] lasers;              // Lasers that Boss will shoot (they need to have a scale of 0 on the Z Axis).
    public GameObject spawnLaser;           // Laser that will be shooted after the laser has ended.
    public float laserShotDelay = 1.0f;     // Shoot delay for the lasers.
    public float laserChargerTime = 1.0f;   // Time it takes to charge the laser.
    public float laserSpeed = 1.0f;         // Speed at which the laser will increase its size.
    public float laserDuration = 1.0f;      // Time the laser will be increasing size before being shooted.
    public float targetDistance = 10.0f;    // Distance at which the Boss will start attacking the player and distance at which it will mantain between the player and itself.

    protected GameObject gameplayPlane;     // gameplayPlane GameObject where the player moves.
    private bool called = false;            // Bool to call part of the LateUpdate function only once.
    private bool cannonDestroyed = false;   // Bool to set it once the cannon of the boss has been destroyed.

    new void Start ()
    {
        base.Start();
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");  // Set gameplayPlane variable.
	}

    void LateUpdate()
    {
        if (Vector3.Project(gameplayPlane.transform.position - transform.position, gameplayPlane.transform.forward).magnitude <= targetDistance || called) // Look for distance to the player.
        {
            if(localMovement && transform.parent != null)
                StayInFrontOf(gameplayPlane, targetDistance, transform.parent); // If the gameObject is moving locally and has a parent means that the parent is part of the Boss, so the parent is moved.
            else
                StayInFrontOf(gameplayPlane, targetDistance, transform);        // else only gameObject is moved.

            if (!called)    // Following code will be called just once.
            {
                if(localMovement)
                    MoveEnemy(transform.position, moveAxis, localMovement); // Start boss movement.
                Invoke("ShootSpawnableAmmo", spawnAmmoDelay);               // Start shooting.
                Invoke("ChargeLaser", laserShotDelay);                      // Start charging the laser.
                called = true;      
            }
            LookAtPlayer();     // Keep looking at Player.
        }
        if(turretBase != null)  // Avoid calling turret base when Boss is destroyed.
        {
            if (turretBase.GetComponentInChildren<BossModuleHealthManager>() == null && !cannonDestroyed)   // The cannon was destroyed. Stop all laser sounds.
            {
                cannonDestroyed = true;
                AudioManager.Instance.Stop("LaserBeamCharge");
                AudioManager.Instance.Stop("LaserBeamShoot");
            }
        }
        else // The turret base was destroyed. Stop all laser sounds.
        {
            cannonDestroyed = true;
            AudioManager.Instance.Stop("LaserBeamCharge");
            AudioManager.Instance.Stop("LaserBeamShoot");
        }    
    }

    /// <summary>
    /// Overriden function, stops the recursive call if the thrusters of the Boss are destroyed.
    /// </summary>
    protected override void MoveEnemy(Vector3 pivot, Vector3 axisMovement, bool local)
    {
        foreach(GameObject thruster in thrusters)
        {
            if(thruster != null)
            {
                base.MoveEnemy(pivot, axisMovement, local);
                break;
            }
        }
    }

    /// <summary>
    /// Funtion to charge the laser, plays the charging sound and sets active the lasers. Fires the lasers with the time provided as variable.
    /// </summary>
    void ChargeLaser()
    {
        if (!cannonDestroyed)   // Avoid calling this code when the cannon was destroyed.
        {
            AudioManager.Instance.Play("LaserBeamCharge");
            foreach (Transform laser in lasers)
            {
                if (laser == null)  // Avoid calling a laser of the array that was destoyed.
                    return;
                laser.gameObject.SetActive(true);
            }
            Invoke("FireLaser", laserChargerTime);
        }
    }

    /// <summary>
    /// Fires the laser: play the firing sound and call the couroutine.
    /// </summary>
    void FireLaser()
    {
        if (!cannonDestroyed)   // Avoid calling this code when the cannon was destroyed.
        {
            AudioManager.Instance.Play("LaserBeamShoot");
            StartCoroutine("LaserCouroutine");
        }
    }

    /// <summary>
    /// Increases the size of the laser GameObject at the sapeed provided as variable for the time also provided. At the end calls LaserCoolDown.
    /// </summary>
    IEnumerator LaserCouroutine()
    {
        float t = 0;
        while(t < laserDuration)
        {
            foreach(Transform laser in lasers)
            {
                if (laser == null) // Avoid calling a laser of the array that was destoyed.
                    yield break;
                Vector3 newScale = laser.localScale;
                newScale.z += laserSpeed * Time.deltaTime;
                laser.localScale = newScale;
            }
            t += Time.deltaTime;
            yield return null;
        }
        LaserCoolDown();
    }

    /// <summary>
    /// Resets lasers to initial value and restarts the laser firing secuence after the delay time has passed.
    /// </summary>
    /// <returns></returns>
    void LaserCoolDown()
    {
        foreach (Transform laser in lasers)
        {
            if (laser == null) // Avoid calling a laser of the array that was destoyed.
                return;
            Vector3 newScale = laser.localScale;
            newScale.z = 0;
            laser.localScale = newScale;
            laser.gameObject.SetActive(false);
            var newLaser = Instantiate(spawnLaser, laser.position, laser.rotation);                     // Spawn a copy of the laser on the same point which the lasers are
            newLaser.transform.localScale = new Vector3(laser.lossyScale.x, laser.lossyScale.y, 50);    // Adjust Scale
            Destroy(newLaser, 2.0f);                                                                    // Destroy after 2 seconds
        }
        Invoke("ChargeLaser", laserShotDelay);
    }

    /// <summary>
    /// Overriden function for shooting the spawnable ammo, stops the recursive call if the frontMissileSpawner of the Boss is destroyed.
    /// </summary>
    protected override void ShootSpawnableAmmo()
    {
        if (frontMissileSpawner != null)
        {
            base.ShootSpawnableAmmo();
            if (bulletSpawnPoints.Length > 0)
                Invoke("ShootSpawnableAmmo", spawnAmmoDelay);
        }  
    }
}
