using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that spawns a GameObject when colliding.
/// </summary>
public class SpawnOnTrigger : MonoBehaviour {

    public GameObject spawnObject;      // GameObject to spawn upon collision.
    public float destroyTime = 1.0f;    // Time to destroy the spawned GameObject.

    private void OnTriggerEnter(Collider other)
    {
        SpawnObject(other);
    }

    private void OnTriggerStay(Collider other)
    {
        SpawnObject(other);
    }

    /// <summary>
    /// Function to spawn the new GameObject.
    /// </summary>
    /// <param name="other"> GameObject that collided with gameObject.</param>
    void SpawnObject(Collider other)
    {
        // Spawn the new GameObject on the closest point between other and the gameObject.
        if (other.CompareTag("PlayerCollider"))     
        {
            var s = Instantiate(spawnObject, other.ClosestPointOnBounds(other.transform.position), Quaternion.identity);
            s.transform.localScale = Vector3.one;
            s.transform.parent = transform;
            Destroy(s, destroyTime);
        }
        else
        {
            var s = Instantiate(spawnObject, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
            s.transform.parent = transform;
            Destroy(s, destroyTime);
        }  
    }
}
