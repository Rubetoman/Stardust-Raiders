using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float targetDistance = 10.0f;
    public float enemySpeed = 0.0f;

    private GameObject player;
    private GameObject gameplayPlane;
    private bool following = false;
	// Use this for initialization
	void Start () {
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
    }
	
	// Update is called once per frame
	void LateUpdate () {
		if(Vector3.Project(gameplayPlane.transform.position - transform.position, gameplayPlane.transform.forward).magnitude <= targetDistance && !following)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = gameplayPlane.transform.position.z + targetDistance;
            transform.position = newPosition;
            following = true;
        }
        else if (following)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = gameplayPlane.transform.position.z + targetDistance;
            transform.position = newPosition;
        }
        else
        {
            transform.position += transform.forward * enemySpeed * Time.deltaTime;
        }
	}
}
