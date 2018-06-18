using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController {
    [Header("Boss Parts")]          // Geometry of the Boss GO for referencing
    public Transform turretBase;    // The base of the turret (needs to be pointing forward)
    public GameObject frontMissileSpawner;
    public GameObject[] thrusters;

    [Space(10)]
    [Header("Boss Variables")]
    public float missileDelay = 1.0f;
    [Space(10)]
    [Header("Laser Attack")]
    public Transform[] lasers;
    public GameObject spawnLaser;
    public float laserRotationSpeed = 1.0f;
    public float laserShotDelay = 1.0f;
    public float laserChargerTime = 1.0f;
    public float laserSpeed = 1.0f;
    public float laserDuration = 1.0f;
    public float targetDistance = 10.0f;

    protected GameObject gameplayPlane;
    private bool called = false;
    private bool cannonDestroyed = false;
    // Use this for initialization
    new void Start ()
    {
        base.Start();
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
	}

    // Update is called once per frame
    void LateUpdate()
    {
        if (Vector3.Project(gameplayPlane.transform.position - transform.position, gameplayPlane.transform.forward).magnitude <= targetDistance || called)
        {
            // If the gameObject is moving locally and has a parent means that we want to keep the parent with us
            if(localMovement && transform.parent != null)
                StayInFrontOf(gameplayPlane, targetDistance, transform.parent);
            else
                StayInFrontOf(gameplayPlane, targetDistance, transform);

            if (!called)
            {
                if(localMovement)
                MoveEnemy(transform.position, moveAxis, localMovement);
                Invoke("ShootSpawnableAmmo", missileDelay);
                Invoke("ChargeLaser", laserShotDelay);
                called = true;
            }
            LookAtPlayer();
        }
        if(turretBase != null)
        {
            if (turretBase.GetComponentInChildren<BossModuleHealthManager>() == null && !cannonDestroyed)
            {
                cannonDestroyed = true;
                AudioManager.Instance.Stop("LaserBeamCharge");
                AudioManager.Instance.Stop("LaserBeamShoot");
            }
        }
        else
        {
            cannonDestroyed = true;
            AudioManager.Instance.Stop("LaserBeamCharge");
            AudioManager.Instance.Stop("LaserBeamShoot");
        }    
    }

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

    void ChargeLaser()
    {
        AudioManager.Instance.Play("LaserBeamCharge");
        foreach (Transform laser in lasers)
        {
            if (laser == null)
                return;
            laser.gameObject.SetActive(true);
        }
        Invoke("FireLaser", laserChargerTime);
    }

    void FireLaser()
    {
        AudioManager.Instance.Play("LaserBeamShoot");
        StartCoroutine("LaserCouroutine");
    }

    IEnumerator LaserCouroutine()
    {
        float t = 0;
        while(t < laserDuration)
        {
            foreach(Transform laser in lasers)
            {
                if (laser == null)
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
    /// Resets lasers to initial value
    /// </summary>
    /// <returns></returns>
    void LaserCoolDown()
    {
        foreach (Transform laser in lasers)
        {
            if (laser == null)
                return;
            Vector3 newScale = laser.localScale;
            newScale.z = 0;
            laser.localScale = newScale;
            laser.gameObject.SetActive(false);
            var newLaser = Instantiate(spawnLaser, laser.position, laser.rotation); // Spawn a copy of the laser on the same point which the lasers are
            //newLaser.transform.parent = transform;                                  // Make it a chil of the GameObject which contains the script
            newLaser.transform.localScale = new Vector3(laser.lossyScale.x, laser.lossyScale.y, 50);                  // Adjust Scale
            Destroy(newLaser, 2.0f);                                                // Destroy after 2 seconds
        }
        Invoke("ChargeLaser", laserShotDelay);
    }

    protected override void ShootSpawnableAmmo()
    {
        if (frontMissileSpawner != null)
        {
            base.ShootSpawnableAmmo();
            if (missileSpawnPoints.Length > 0)
                Invoke("ShootSpawnableAmmo", missileDelay);
        }  
    }
}
