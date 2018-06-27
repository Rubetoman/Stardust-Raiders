using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script with base functions for controlling any Enemy. It lets them spawn shoot, move, tarjet at player and stay in front of the player.
/// </summary>
public class EnemyController : MonoBehaviour {
    [Header("Enemy Pointing")]              
    public Transform cannonParent;          // The GameObject that contains the rotating cannons or the GameObject that will look at Player.
    public float cannonRotationSpeed;       // Speed at which the cannon will rotate.
    public float heightOffset;              // Offset for the Y axis of the point where the Player is located and the cannon will look.
    [Space(10)]
    [Header("Spawnable Ammo")]              
    public Transform[] bulletSpawnPoints;   // Transform array for every GameObject that will spawn the spawnable ammo.
    public GameObject spawnBullet;          // GameObject of the ammo that will be spawned.
    public float bulletDestroyTime = 4.0f;  // Time it takes to spawn the ammo. 
    protected int bulletIndex = 0;          // Index of the bullet being spawned.
    protected GameObject bullet;            // GameObject to be spawned.
    [Space(10)]
    [Header("Enemy Movement")]
    public float movementTime = 1.0f;                       // Time that one move takes to be completed.
    public float movementRadius = 10.0f;                    // The radius where the Enemy will move.
    public float movementDelay = 5.0f;                      // Delay time between different moves.
    public Vector3 moveAxis = new Vector3(1.0f, 1.0f, 0.0f);// In which axis will the Enemy move.
    public bool localMovement;                              // If true the enemy will move using local position instead of world position.
    public bool loopMovement = true;                        // If true the Enemy will constantly move, else it will move only once.
    protected Vector3 goalPosition;                         // Position where is going to move the Boss.                    
    protected Vector3 currentPosition;                      // Current position of the Enemy.

    protected GameObject player;                            // Player GameObject.

    protected void Start () {
        player = GameManager.Instance.player;    // Set player variable.
    }

    /// <summary>
    /// Function to keep the gameObject looking at the Player.
    /// </summary>
    protected virtual void LookAtPlayer()
    {
        if (player == null)     // Avoid executing code if player variable is null.
        {
            Debug.LogWarning("Player couldn't be found. Searching again...");
            player = GameManager.Instance.player;    // Set player variable.
            if (player != null)
                Debug.LogWarning("Player found!");
            else
                return;
        }
        // Look at the player: Take player position minus the offset and then make the cannon rotate to that position.
        var playerPos = new Vector3(player.transform.position.x, player.transform.position.y - heightOffset, player.transform.position.z);
        if(cannonParent!=null)
            cannonParent.rotation = Quaternion.RotateTowards(cannonParent.rotation, Quaternion.LookRotation(playerPos - cannonParent.position), cannonRotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Function to shoot the spawnable ammo.
    /// </summary>
    protected virtual void ShootSpawnableAmmo()
    {
        if (bulletSpawnPoints.Length <= 0)  // Exit function if there are no spawn points.
        {
            Debug.LogWarning(gameObject.name + ": There are no spawn points assigned to spawn bullets");
            return;
        }
        if (spawnBullet == null)            // Exit function if there is no GameObject to spawn.  
        {
            Debug.LogWarning(gameObject.name + ": There is no bullet GameObject assigned to be spawned");
            return;
        }

        Transform spawnPoint = bulletSpawnPoints[bulletIndex];  // Get Transform of the current index of the spawn points.
        bulletIndex++;                                          // Increase index.
        if (bulletIndex >= bulletSpawnPoints.Length)            // Reached last spawn point.
        {
            bulletIndex = 0;                                    // Reset the bullet index.
        }
        if(spawnPoint != null)
        {
            bullet = Instantiate(spawnBullet, spawnPoint.position, spawnPoint.rotation);    //Spawn the bullet.
            if (transform.parent != null)
                bullet.transform.parent = transform.parent;     // Set the enemy's parent as parent for the spawned bulled.
            else
                bullet.transform.parent = transform;            // Set the enemy as parent for the spawned bulled.
            Destroy(bullet, bulletDestroyTime);
        }
    }

    /// <summary>
    /// Function to 
    /// Function to keep the desired GameObject at the desired distance in front of a target.
    /// </summary>
    /// <param name="target">The GameObject that will be keept in front of objToMove. </param>
    /// <param name="targetDistance"> Distance in the Z axis between objToMove and the target. </param>
    /// <param name="objToMove"> Transform of the object to keep in front of the target. </param>
    protected void StayInFrontOf(GameObject target, float targetDistance, Transform objToMove)
    {    
        Vector3 newPosition = objToMove.position;
        newPosition.z = target.transform.position.z + targetDistance;
        objToMove.transform.position = newPosition;
    }

    /// <summary>
    /// Function to stop the movement of the Enemy.
    /// </summary>
    public void StopMoving()
    {
        loopMovement = false;
    }

    /// <summary>
    /// Function to make the enemy translate, it will move to another point inside a sphere of radius 1 with the initial position as center.
    /// </summary>
    /// <param name="pivot"> Point used as center of the sphere. </param>
    /// <param name="axisMovement"> Multiplier for each axis, if 0 won't move in that axis. </param>
    /// <param name="local"> True if the movement will be using localPosition, false if world. </param>
    protected virtual void MoveEnemy(Vector3 pivot, Vector3 axisMovement, bool local)
    {
        Vector3 movementVector = Random.insideUnitSphere * movementRadius;          // Get the new random movement vector.
        movementVector.x *= axisMovement.x;                                         
        movementVector.y *= axisMovement.y;
        movementVector.z *= axisMovement.z;
        goalPosition = pivot + movementVector;                                      // Set the new point to move towards.
        currentPosition = (local)? transform.localPosition : transform.position;    
        StartCoroutine(ActuallyMoveEnemy(pivot, axisMovement, local));              // Call to the function that will actually make a smooth movement.
    }

    /// <summary>
    /// Function to smoothly move enemy to a new position.
    /// </summary>
    IEnumerator ActuallyMoveEnemy(Vector3 pivot, Vector3 axisMovement, bool local)
    {
        float t = 0.0f;
        while (t < movementTime)
        {
            t += Time.deltaTime;
            goalPosition.z = (local)? transform.localPosition.z : transform.position.z;
            if(local)
                transform.localPosition = Vector3.Lerp(currentPosition, goalPosition, t / movementTime);
            else
                transform.position = Vector3.Lerp(currentPosition, goalPosition, t / movementTime);
            yield return null;
        }
        if (loopMovement){                                      // Recursive call after a wait time.
            yield return new WaitForSeconds(movementDelay);
            MoveEnemy(pivot, axisMovement, local);
        }
    }
}
