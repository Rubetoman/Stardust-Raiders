using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [Header("Enemy Pointing")]              //Make the enemy weapons to point the player
    public Transform cannonParent;          // The GO that contains the rotating cannons
    public float cannonRotationSpeed;
    public float heightOffset = 4.0f;
    [Space(10)]
    [Header("Spawnable Ammo")]              //Ammo that is going to be spawnable from a spawn point
    public Transform[] missileSpawnPoints;
    public GameObject spawnMissile;
    public float missileDestroyTime = 4.0f;
    protected int missileIndex = 0;
    protected GameObject missile;
    [Space(10)]
    [Header("Enemy Movement")]
    public float movementTime = 1.0f;       // Time that one move takes
    public float movementRadius = 10.0f;    // the maximun radius of movement is going to use
    public float movementDelay = 5.0f;      // Delay time between different moves
    public Vector3 moveAxis = new Vector3(1.0f, 1.0f, 0.0f);
    public bool loopMovement = true;
    protected Vector3 goalPosition;
    protected Vector3 currentPosition;

    protected GameObject player;
    // Use this for initialization
    protected void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
    protected virtual void LookAtPlayer()
    {
        // Look at the player: Take player position minus the offset and then make the cannon rotate to that position.
        var playerPos = new Vector3(player.transform.position.x, player.transform.position.y - heightOffset, player.transform.position.z);
        cannonParent.rotation = Quaternion.RotateTowards(cannonParent.rotation, Quaternion.LookRotation(playerPos - cannonParent.position), cannonRotationSpeed * Time.deltaTime);
    }

    protected virtual void ShootSpawnableAmmo()
    {
        if (missileSpawnPoints.Length <= 0)
            return;

        Transform spawnPoint = missileSpawnPoints[missileIndex];
        missileIndex++;
        if (missileIndex >= missileSpawnPoints.Length)
        {
            missileIndex = 0;
        }
        if(spawnPoint != null && spawnMissile != null)
        {
            missile = Instantiate(spawnMissile, spawnPoint.position, spawnPoint.rotation);
            missile.transform.parent = transform;
            Destroy(missile, missileDestroyTime);
        }
    }

    protected void StandInFrontOf(GameObject focused, float targetDistance)
    {
        Vector3 newPosition = transform.position;
        newPosition.z = focused.transform.position.z + targetDistance;
        transform.position = newPosition;
    }

    void StopMoving()
    {
        loopMovement = false;
    }

    protected virtual void MoveEnemy(Vector3 pivot, Vector3 axisMovement)
    {
        Vector3 movementVector = Random.insideUnitSphere * movementRadius;
        movementVector.x *= axisMovement.x;
        movementVector.y *= axisMovement.y;
        movementVector.z *= axisMovement.z;
        goalPosition = pivot + movementVector;
        currentPosition = transform.position;
        StartCoroutine(ActuallyMoveEnemy(pivot, axisMovement));
    }

    IEnumerator ActuallyMoveEnemy(Vector3 pivot, Vector3 axisMovement)
    {
        
        float t = 0.0f;
        while (t < movementTime)
        {
            t += Time.deltaTime;
            goalPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(currentPosition, goalPosition, t / movementTime);
            yield return null;
        }
        if (loopMovement){
            yield return new WaitForSeconds(movementDelay);
            MoveEnemy(pivot, axisMovement);
        }
    }
}
