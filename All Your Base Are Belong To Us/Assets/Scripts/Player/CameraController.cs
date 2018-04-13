using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 followingRatio = Vector2.one;

    private Vector3 newPos;

	void LateUpdate () {
        //localPosition is used to use the position relative to the parent (gameplayPlane)
        newPos = objectToFollow.localPosition; //Pick-up the ship position
        //Modify the position of the ship depending on the ratio
        newPos.x *= followingRatio.x;
        newPos.y *= followingRatio.y;
        newPos.z = transform.localPosition.z; //Z axis is constant
        transform.localPosition = newPos; //Apply the new position to the camera
    }
}
