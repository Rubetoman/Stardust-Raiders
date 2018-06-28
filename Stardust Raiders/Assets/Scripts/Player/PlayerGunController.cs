using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for the type of gun.
/// </summary>
public enum GunType
{
    Single,
    Dual,
    Triple,
}

/// <summary>
/// Script for Player's ship to controll his guns.
/// </summary>
public class PlayerGunController : MonoBehaviour {

    public GameObject mainGun;          // Place where the Main Gun is located.
    public GameObject secondaryGun1;    // Place where one of the Secondary Guns is located.
    public GameObject secondaryGun2;    // Place where one of the Secondary Guns is located.
    public Rigidbody bullet;            // The bullet prefab to spawn.
    public float velocity = 10.0f;      // Velocity of the bullets.
    public float yOffset = 0.0f;        // Will modify the bullets z component of the spawn point.
    public float destroyTime = 2.0f;    // Time it takes to destroy the bullet.

	void FixedUpdate () {

        if (Input.GetButtonDown("Fire1"))                                                   // Get Input for shooting.
        {
            switch (GameManager.Instance.playerInfo.gunType)
            {
                case GunType.Single:
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(10);   // Set standard damage.  
                    Shoot(mainGun.transform);                                               // Shoot a bullet.
                    break;
                case GunType.Dual:
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(20);   //Change damage, so only the first bullet makes damage.
                    Shoot(secondaryGun1.transform);                                         // Shoot first bullet.
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(0);    // Next bullet doesn't make damage.
                    Shoot(secondaryGun2.transform);                                         // Shoot second bullet.
                    break;
                case GunType.Triple:
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(30);   //Change damage, so only the first bullet makes damage.
                    Shoot(mainGun.transform);                                               // Shoot first bullet.
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(0);    // Next two bullets don't make damage.
                    Shoot(secondaryGun1.transform);                                         // Shoot second bullet.
                    Shoot(secondaryGun2.transform);                                         // Shoot third bullet.
                    break;
            }
        }
	}

    /// <summary>
    /// Function that shoots the bullets.
    /// </summary>
    /// <param name="source"> Transform of the object from from where is going to be shoot.</param>
    void Shoot(Transform source)
    {
        // Instantiate a bullet on the point where the transform is located + the modification given to the Z.
        Rigidbody newBullet = Instantiate(bullet, new Vector3(source.position.x, source.position.y + yOffset, source.position.z), source.rotation) as Rigidbody;
        // Add force to the bullet to make it advance forward.
        newBullet.AddForce(source.forward * velocity, ForceMode.VelocityChange);
        // Destroy the bullet after the time given.
        Destroy(newBullet.gameObject, destroyTime);
    }
}
