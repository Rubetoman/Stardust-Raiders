using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {
    public Transform turretBase;
    public Transform cannonParent;
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
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("ChargeLaser", laserShotDelay);
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
}
