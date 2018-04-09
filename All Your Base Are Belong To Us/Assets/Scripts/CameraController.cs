using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 followingRatio = Vector2.one;
    //public Vector3 offset = Vector3.zero;
    //private Vector3 newPos;

	// Use this for initialization
	void Start () {
        /*newPos = objectToFollow.position;
        newPos.x *= followingRatio.x;
        newPos.y *= followingRatio.y;
        newPos.z = transform.position.z;
        offset = objectToFollow.transform.position - transform.position;
        offset.z = 0;
        transform.position = newPos - offset;*/
        //transform.position = new Vector3(objectToFollow.position.x, objectToFollow.position.y, transform.position.z);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 newPos = objectToFollow.position;
        newPos.x *= followingRatio.x;
        newPos.y *= followingRatio.y;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
