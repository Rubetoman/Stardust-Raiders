using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour {

    public GameObject spawnObject;
    public float destroyTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Destroy(Instantiate(spawnObject, other.ClosestPointOnBounds(transform.position), Quaternion.identity), destroyTime);
    }

    private void OnTriggerStay(Collider other)
    {
        Destroy(Instantiate(spawnObject, other.ClosestPointOnBounds(transform.position), Quaternion.identity),destroyTime);
    }
}
