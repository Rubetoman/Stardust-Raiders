using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : EnemyController
{
    [Header("Turret Variables")]
    public float missileDelay = 1.0f;
    public float targetDistance = 300.0f;            // Distance at which the enemy will start shooting
    public Transform turret;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        Invoke("ShootSpawnableAmmo", missileDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform != null)
            LookAtPlayer();
    }

    /// <summary>
    /// Overrides the previous function, it changes because turrets are formed by two rotating parts. One for Y axis and other for X axis.
    /// </summary>
    protected override void LookAtPlayer()
    {
        // Look at the player: It's done rotating two pieces, for the y axis rotation the cannonParent is used and for the x axis the turret
        // Y axis
        var cannonTargetPoint = new Vector3(player.transform.position.x, cannonParent.position.y, player.transform.position.z) - cannonParent.position;
        var targetRotation = Quaternion.LookRotation(cannonTargetPoint, Vector3.up);
        cannonParent.localRotation = Quaternion.Slerp(cannonParent.localRotation, targetRotation, Time.deltaTime * cannonRotationSpeed);
        
        // X axis
        //Take player position minus the offset and then make the cannon rotate to that position.
        var turretTargetPoint = new Vector3(turret.position.x, player.transform.position.y - heightOffset, player.transform.position.z) - turret.position;
        var turretTargetRotation = Quaternion.LookRotation(turretTargetPoint, Vector3.up);
        turret.localRotation = Quaternion.Slerp(turret.localRotation, turretTargetRotation, Time.deltaTime * cannonRotationSpeed);
    }

    protected override void ShootSpawnableAmmo()
    {
        if (Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= targetDistance)
        {
            base.ShootSpawnableAmmo();
        }


        if (missileSpawnPoints.Length > 0)
            Invoke("ShootSpawnableAmmo", missileDelay);
    }

}
