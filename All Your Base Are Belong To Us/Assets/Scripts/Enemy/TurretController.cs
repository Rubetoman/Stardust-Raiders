using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll enemy turrets behaviour. It lets them move the turret using two degrees of freedom in two different GameObjects (Base and Turret).
/// Inherits form EnemyController.
/// </summary>
public class TurretController : EnemyController
{
    [Header("Turret Variables")]
    public float shootDelay = 1.0f;         // Delay between bullet spawn. 
    public float targetDistance = 300.0f;   // Distance at which the enemy will start shooting
    public Transform turret;

    new void Start()
    {
        base.Start();
        Invoke("ShootSpawnableAmmo", shootDelay);   // Start shooting.
    }

    void Update()
    {
        if (transform != null)
            LookAtPlayer();
        else
            Debug.LogWarning(gameObject.name + ": Doesn't contain Transform component.");
    }

    /// <summary>
    /// Overrides the previous function, it changes due to turrets being composed by two rotating GameObjects. One for Y axis and other for X axis.
    /// </summary>
    protected override void LookAtPlayer()
    {
        // Look at the player: It's done rotating two pieces, for the y axis rotation the cannonParent is used and for the x axis the turret.
        // Y axis:
        var cannonTargetPoint = new Vector3(player.transform.position.x, cannonParent.position.y, player.transform.position.z) - cannonParent.position;
        var targetRotation = Quaternion.LookRotation(cannonTargetPoint, Vector3.up);
        cannonParent.localRotation = Quaternion.Slerp(cannonParent.localRotation, targetRotation, Time.deltaTime * cannonRotationSpeed);
        
        // X axis:
        //Take player position minus the offset and then make the cannon rotate to that position.
        var turretTargetPoint = new Vector3(turret.position.x, player.transform.position.y - heightOffset, player.transform.position.z) - turret.position;
        var turretTargetRotation = Quaternion.LookRotation(turretTargetPoint, Vector3.up);
        turret.localRotation = Quaternion.Slerp(turret.localRotation, turretTargetRotation, Time.deltaTime * cannonRotationSpeed);
    }

    /// <summary>
    /// Overriden function to shoot bullets, they are only spawned if the player distance is lower than targetDistance.
    /// </summary>
    protected override void ShootSpawnableAmmo()
    {
        if (Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= targetDistance)
            base.ShootSpawnableAmmo();

        if (bulletSpawnPoints.Length > 0)   // Avoid to keep shooting if there are no spawn points.
            Invoke("ShootSpawnableAmmo", shootDelay);
        else
            Debug.Log(gameObject.name + ": spawn points could't be found.");
    }

}
