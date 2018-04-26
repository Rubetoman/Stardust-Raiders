using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController {
    [Header("Boss Parts")]          // Geometry of the Boss GO for referencing
    public Transform turretBase;    // The base of the turret (needs to be pointing forward)
    public GameObject frontMissileSpawner;
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
    private GameObject gameplayPlane;
    public float targetDistance = 10.0f;
    // Use this for initialization
    new void Start ()
    {
        base.Start();
        originalPosition = transform.position;
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
        Invoke("ChargeLaser", laserShotDelay);
        Invoke("ShootSpawnableAmmo", missileDelay);
        Invoke("MoveEnemy", movementDelay);
	}

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        StandInFrontOf(gameplayPlane, targetDistance);
    }

    void ChargeLaser()
    {
        foreach (Transform laser in lasers)
        {
            if (laser == null)
                continue;
            laser.gameObject.SetActive(true);
        }
        //Play some cool effect for charging, particles, etc.
        Invoke("FireLaser", laserChargerTime);
    }

    void FireLaser()
    {
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
                    continue;
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
                continue;
            Vector3 newScale = laser.localScale;
            newScale.z = 0;
            laser.localScale = newScale;
            laser.gameObject.SetActive(false);
            var newLaser = Instantiate(spawnLaser, laser.position, laser.rotation); // Spawn a copy of the laser on the same point which the lasers are
            newLaser.transform.parent = transform;                                  // Make it a chil of the GameObject which contains the script
            newLaser.transform.localScale = new Vector3(1, 1, 10);                  // Adjust Scale
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
