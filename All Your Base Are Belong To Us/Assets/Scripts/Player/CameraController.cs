using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 stopDistance = new Vector2(40,10);
    public Transform limitPlane;

    private Vector3 newPos;
    private Vector2 limit;
    private void Start()
    {
        // Take the scale to calculate whre the movement of the player is limited
        limit = limitPlane.localScale;
        limit.x /= 2;
        limit.y /= 2;
    }

    void Update () {
        // Get position of the object we want to follow
        newPos = objectToFollow.localPosition;
        // The gameObject will move always inside the limits imposed and the Z axis will be static. Once you are near the limit the camera will stop following.
        transform.localPosition = new Vector3(Mathf.Clamp(newPos.x,-(limit.x - stopDistance.x), limit.x - stopDistance.x), Mathf.Clamp(newPos.y, -(limit.y - stopDistance.y), limit.y - stopDistance.y), transform.localPosition.z);
    }
}
