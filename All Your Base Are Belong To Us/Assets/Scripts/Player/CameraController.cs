using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 limitOffset = new Vector2(0.5f, 0.5f);
    public GameObject limitPlane;

    private Vector3 newPos;

    private void Start()
    {

    }

    void LateUpdate () {
        //localPosition is used to use the position relative to the parent (gameplayPlane)
        newPos = objectToFollow.localPosition; //Pick-up the ship position
        //Modify the position of the ship depending on the distance to the center of the plane
        var distanceToCenterX = Mathf.Abs(transform.localPosition.x - limitPlane.transform.localPosition.x);
        var distanceToCenterY = Mathf.Abs(transform.localPosition.y - limitPlane.transform.localPosition.y);
        newPos.x *= 1 + limitOffset.x - distanceToCenterX / (limitPlane.transform.localScale.x / 2);
        newPos.y *= 1 + limitOffset.y - distanceToCenterY / (limitPlane.transform.localScale.y / 2);
        newPos.z = transform.localPosition.z; //Z axis is constant
        transform.localPosition = newPos; //Apply the new position to the camera
    }
}
