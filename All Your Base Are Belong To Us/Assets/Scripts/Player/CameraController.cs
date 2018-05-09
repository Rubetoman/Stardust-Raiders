using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 stopDistance = new Vector2(30,10);

    private Vector3 newPos;

    private void Start()
    {

    }

    void Update () {
        // Get position of the object we want to follow
        newPos = objectToFollow.localPosition;
        // The gameObject will move always inside the limits imposed and the Z axis will be static
        transform.localPosition = new Vector3(Mathf.Clamp(newPos.x, -stopDistance.x, stopDistance.x), Mathf.Clamp(newPos.y, -stopDistance.y, stopDistance.y), transform.localPosition.z);
    }
}
