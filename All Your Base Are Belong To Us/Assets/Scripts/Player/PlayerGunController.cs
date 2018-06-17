using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Single,
    Dual,
    Triple,
}
public class PlayerGunController : MonoBehaviour {

    public GameObject mainGun;          // Place where the Main Gun is located
    public GameObject secondaryGun1;
    public GameObject secondaryGun2;
    public Rigidbody bullet;            // The bullet prefab
    public float velocity = 10.0f;      // Velocity of the bullets
    public float yOffset = 0.0f;        // Will modify the bullets z component of the spawn point
    public float destroyTime = 2.0f;    // Time it takes to destroy the bullet

	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetButtonDown("Fire1"))
        {
            switch (GameManager.Instance.playerInfo.gunType)
            {
                case GunType.Single:
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(10);
                    Shoot(mainGun.transform);
                    break;
                case GunType.Dual:
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(20); //Increase damage, only first bullet will actually make damage
                    Shoot(secondaryGun1.transform);
                    Shoot(secondaryGun2.transform);
                    break;
                case GunType.Triple:
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(30); //Increase damage, only first bullet will actually make damage
                    Shoot(mainGun.transform);
                    bullet.gameObject.GetComponent<BulletController>().SetAllDamages(0);
                    Shoot(secondaryGun1.transform);
                    Shoot(secondaryGun2.transform);
                    break;
            }
        }
	}

    /// <summary>
    /// Function that shoots the bullets
    /// </summary>
    /// <param name="source">Transform of the object from whre is going to be shoot</param>
    void Shoot(Transform source)
    {
        //Instantiate a bullet on the point where the transform is locates + the modification given to the Z
        Rigidbody newBullet = Instantiate(bullet, new Vector3(source.position.x, source.position.y + yOffset, source.position.z), source.rotation) as Rigidbody;
        //Make the bullet to advance
        newBullet.AddForce(source.forward * velocity, ForceMode.VelocityChange);
        //Destroy the bullet after the time given
        Destroy(newBullet.gameObject, destroyTime);
    }
}
