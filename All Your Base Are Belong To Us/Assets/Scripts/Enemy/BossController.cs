using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
    [Header("Boss Parts")]
    public Transform turretBase;
    public Transform cannonParent;
    [Space(10)]
    [Header("Boss Movement")]
    //public float movementSpeed = 1.0f;
    public float movementTime = 1.0f;
    public float movementRadius = 10.0f;
    public float movementDelay = 5.0f;
    private Vector3 originalPosition;
    private Vector3 goalPosition;
    private Vector3 currentPosition;
    [Space(10)]
    [Header("Spawnable Ammo")]
    public Transform[] missileSpawnPoints;
    public GameObject spawnMissile;
    public float missileDelay = 1.0f;
    private int missileIndex = 0;
    [Space(10)]
    [Header("Laser Attack")]
    public Transform[] lasers;
    public GameObject spawnLaser;
    public float laserRotationSpeed = 1.0f;
    public float laserShotDelay = 1.0f;
    public float laserChargerTime = 1.0f;
    public float laserSpeed = 1.0f;
    public float laserDuration = 1.0f;

    private GameObject player;

	// Use this for initialization
	void Start ()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("ChargeLaser", laserShotDelay);
        Invoke("ShootMissile", missileDelay);
        Invoke("MoveBoss", movementDelay);
	}

    // Update is called once per frame
    void Update()
    {
        // Look at the player
        //cannonParent.transform.rotation = Quaternion.LookRotation(player.transform.position - cannonParent.position);
        //turretBase.rotation = Quaternion.LookRotation(Vector3.Project(player.transform.position - cannonParent.position, turretBase.forward));
        cannonParent.rotation = Quaternion.RotateTowards(cannonParent.rotation, Quaternion.LookRotation(player.transform.position - cannonParent.position), laserRotationSpeed * Time.deltaTime);
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
            Destroy(newLaser, 1.0f);                                                // Destroy after 1 second
        }
        Invoke("ChargeLaser", laserShotDelay);
    }

    void ShootMissile()
    {
        Transform spawnPoint = missileSpawnPoints[missileIndex];
        missileIndex++;
        if(missileIndex >= missileSpawnPoints.Length || spawnPoint == null)
        {
            missileIndex = 0;
        }
        var missile = Instantiate(spawnMissile, spawnPoint.position, spawnPoint.rotation);
        missile.transform.parent = transform;
        Destroy(missile, 6.0f);
        if(missileSpawnPoints.Length > 0)
            Invoke("ShootMissile", missileDelay);
    }

    void MoveBoss()
    {
        Vector3 movementVector = Random.insideUnitSphere * movementRadius;
        movementVector.y = originalPosition.y;
        StartCoroutine("ActuallyMoveBoss");
    }

    IEnumerator ActuallyMoveBoss()
    {
        float t = 0.0f;
        while (t < movementTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPosition, goalPosition, t / movementTime);
            yield return null;
        }
        Invoke("MoveBoss", movementDelay);
    }
}
