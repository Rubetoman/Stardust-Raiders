using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour {

    public GameObject spawnObject;
    public float destroyTime = 1.0f;

    private float cooldownTimer = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        SpawnObject(other);
    }

    private void OnTriggerStay(Collider other)
    {
        SpawnObject(other);
    }

    void SpawnObject(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(Instantiate(spawnObject, other.ClosestPointOnBounds(other.transform.position), Quaternion.identity), destroyTime);
        }
        else
        {
            var s = Instantiate(spawnObject, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
            s.transform.parent = transform;
            Destroy(s, destroyTime);
        }  
    }
}
