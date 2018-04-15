using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float targetDistance = 10.0f;
    public float enemySpeed = 0.0f;

    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void LateUpdate () {
		if(Vector3.Project(player.transform.position - transform.position, player.transform.forward).magnitude <= targetDistance)
        {
            /*(player.transform.position - transform.position, player.transform.forward).normalized
            transform.position += Vector3.Project(player.transform.position - transform.position, player.transform.forward)
                - Vector3.Project(transform.position, player.transform.forward);*/
            Vector3 newPosition = transform.position;
            newPosition.z = player.transform.position.z + targetDistance;
            transform.position = newPosition;
        }
        else
        {
            transform.position += transform.forward * enemySpeed * Time.deltaTime;
        }
	}
}
