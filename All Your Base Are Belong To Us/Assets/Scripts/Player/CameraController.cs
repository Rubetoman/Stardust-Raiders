using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform objectToFollow;
    public Vector2 stopDistance = new Vector2(40,10);
    public Transform limitPlane;
    public bool rotateCamera = true;

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
        if(rotateCamera && LevelManager.Instance.GetCurrentSector().playerMovement && GameManager.Instance.gameState == GameManager.StateType.Play) //Avoid rotating player camera when you can't move (Either block by LevelManager or Game Paused)
            RotateCameraOnShipMovement();
    }

    /// <summary>
    /// This function will calculate an appropriate rotation for the camera on ship movement
    /// </summary>
    private void RotateCameraOnShipMovement()
    {
        float verticalRot = Input.GetAxis("Vertical");      // Get vertical input
        float horizontalRot = Input.GetAxis("Horizontal");  // Get horizontal input
        Quaternion startRotation = transform.localRotation; // Get actual rotation
        Quaternion newRotation = transform.localRotation;
        // Calculate new rotations for x and z
        newRotation.x = (startRotation * Quaternion.AngleAxis(-verticalRot * (0.1f), Vector3.right)).x;
        newRotation.z = (startRotation * Quaternion.AngleAxis(horizontalRot * (0.1f), Vector3.forward)).z;
        // Apply this new rotations clamping to avoid to much rotation
        transform.localRotation = new Quaternion(Mathf.Clamp(newRotation.x, -0.02f, 0.02f), 0.0f, Mathf.Clamp(newRotation.z, -0.02f, 0.02f), transform.localRotation.w);
        // Return to normal rotation
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, limitPlane.transform.localRotation, Time.deltaTime * 2.5f);
    }
}
