using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 followingRatio = Vector2.one;
    public Vector3 offset = Vector3.one;
    private Vector3 newPos;

	// Use this for initialization
	void Start () {
        offset = transform.position - objectToFollow.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        newPos = objectToFollow.position;
        newPos.x *= followingRatio.x;
        //newPos.x += offset.x;
        //newPos.y *= followingRatio.y;
        //newPos.y += offset.y;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
