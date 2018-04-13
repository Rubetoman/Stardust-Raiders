using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float targetDistance = 10.0f;

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void LateUpdate () {
		if((player.transform.position - transform.position).magnitude <= targetDistance)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = player.transform.position.z + targetDistance;
            transform.position = newPosition;
        }
	}
}
