using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 limitOffset = new Vector2(0.5f, 0.5f);
    public GameObject limitPlane;

    private Vector3 newPos;
    private Bounds limitBounds;

    private void Start()
    {
        limitBounds = limitPlane.GetComponent<Renderer>().bounds;
    }

    void LateUpdate () {
        //localPosition is used to use the position relative to the parent (gameplayPlane)
        newPos = objectToFollow.localPosition; //Pick-up the ship position
        //Modify the position of the ship depending on the distance to the center of the plane
        var distanceToCenterX = Mathf.Abs(transform.localPosition.x - limitBounds.center.x);
        var distanceToCenterY = Mathf.Abs(transform.localPosition.y - limitBounds.center.y);
        newPos.x *= 1 + limitOffset.x - distanceToCenterX / limitBounds.max.x;
        newPos.y *= 1 + limitOffset.y - distanceToCenterY / limitBounds.max.y;
        newPos.z = transform.localPosition.z; //Z axis is constant
        transform.localPosition = newPos; //Apply the new position to the camera
    }
}
