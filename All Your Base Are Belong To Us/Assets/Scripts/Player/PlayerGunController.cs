using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour {

    public Rigidbody bullet;            // The bullet prefab
    public float velocity = 10.0f;      // Velocity of the bullets
    public float zModification = 1.0f;  // Will modify the bullets z component of the spawn point
    public float destroyTime = 2.0f;    // Time it takes to destroy the bullet

	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetButtonDown("Fire1"))
        {
            //Instantiate a bullet on the point where the transform is locates + the modification given to the Z
            Rigidbody newBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z + zModification), transform.rotation) as Rigidbody;
            //Make the bullet to advance
            newBullet.AddForce(transform.forward * velocity, ForceMode.VelocityChange);
            //Destroy the bullet after the time given
            Destroy(newBullet.gameObject, destroyTime);
        }
	}
}
