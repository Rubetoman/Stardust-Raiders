using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    public Transform cannonParent;  // The GO that contains the rotating cannons
    public Transform[] missileSpawnPoints;
    public GameObject spawnMissile;
    public float missileDelay = 1.0f;
    public float missileDestroyTime = 4.0f;
    public float cannonRotationSpeed;
    private int missileIndex = 0;
    private GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("ShootMissile", missileDelay);
    }
	
	// Update is called once per frame
	void Update () {
        LookAtPlayer();
	}

    void LookAtPlayer()
    {
        // Look at the player: Take player position minus the offset and then make the cannon rotate to that position.
        cannonParent.rotation = Quaternion.RotateTowards(cannonParent.rotation, Quaternion.LookRotation(player.transform.position - cannonParent.position), cannonRotationSpeed * Time.deltaTime);
    }

    void ShootMissile()
    {
        Transform spawnPoint = missileSpawnPoints[missileIndex];
        missileIndex++;
        if (missileIndex >= missileSpawnPoints.Length || spawnPoint == null)
        {
            missileIndex = 0;
        }
        var missile = Instantiate(spawnMissile, spawnPoint.position, spawnPoint.rotation);
        missile.transform.parent = transform;
        Destroy(missile, missileDestroyTime);
        if (missileSpawnPoints.Length > 0)
            Invoke("ShootMissile", missileDelay);
    }
}
