using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform objectToFollow;
    public Vector2 stopDistance = new Vector2(8, 6);
    public Transform limitPlane;
    public bool rotateCamera = true;
    public Vector3 positionOffSet;

    private Vector3 newPos;
    private Vector2 limit;
    private Vector3 initialOffSet;
    private void Start()
    {
        // Take the scale to calculate whre the movement of the player is limited
        limit = limitPlane.localScale;
        limit.x /= 2;
        limit.y /= 2;
        initialOffSet = positionOffSet;
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");  // Get horizontal input
        float vertical = Input.GetAxis("Vertical");      // Get vertical input
        // Get position of the object we want to follow
        newPos = objectToFollow.localPosition + positionOffSet;
        // The gameObject will move always inside the limits imposed and the Z axis will be static. Once you are near the limit the camera will stop following.
        transform.localPosition = new Vector3(Mathf.Clamp(newPos.x, -(limit.x - stopDistance.x), limit.x - stopDistance.x), Mathf.Clamp(newPos.y, -(limit.y - stopDistance.y), limit.y - stopDistance.y), transform.localPosition.z);
        if (rotateCamera && LevelManager.Instance.GetCurrentSector().playerMovement && GameManager.Instance.gameState == GameManager.StateType.Play) //Avoid rotating player camera or updating offset when you can't move (Either block by LevelManager or Game Paused)
        {
            RotateCameraOnShipMovement(horizontal, vertical);
            ChangeOffset(horizontal, vertical);
        }
    }

    /// <summary>
    /// This function will calculate an appropriate rotation for the camera on ship movement
    /// </summary>
    /// <param name="horizontal"> Horizontal Input</param>
    /// <param name="vertical"> Vertical Input</param>
    private void RotateCameraOnShipMovement(float horizontalRot, float verticalRot)
    {

        Quaternion startRotation = transform.localRotation; // Get actual rotation
        Quaternion newRotation = transform.localRotation;
        // Calculate new rotations for x and z
        newRotation.x = (startRotation * Quaternion.AngleAxis(-verticalRot * (0.1f), Vector3.right)).x;
        newRotation.y = (startRotation * Quaternion.AngleAxis(horizontalRot * (0.1f), Vector3.up)).y;
        newRotation.z = (startRotation * Quaternion.AngleAxis(horizontalRot * (0.1f), Vector3.forward)).z;
        // Apply this new rotations clamping to avoid to much rotation
        transform.localRotation = new Quaternion(Mathf.Clamp(newRotation.x, -0.02f, 0.02f), Mathf.Clamp(newRotation.y, -0.02f, 0.02f), Mathf.Clamp(newRotation.z, -0.02f, 0.02f), transform.localRotation.w);
        // Return to normal rotation
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, limitPlane.transform.localRotation, Time.deltaTime * 2.5f);
    }


    /// <summary>
    /// This function updates the offset depending on the position of the spaceship. This way the reticules are always visible.
    /// </summary>
    /// <param name="horizontal"> Horizontal Input</param>
    /// <param name="vertical"> Vertical Input</param>
    private void ChangeOffset(float horizontal, float vertical)
    {
        if (positionOffSet.y != 0)  // If the vertical offSet is 0 don't make extra computations.
        {
            var verticalPos = limitPlane.GetComponent<PlayerLimitManager>().GetPlayerLocationInPlane(DivideType.Up_Down);
            switch (verticalPos)
            {
                case "up":
                    positionOffSet = Vector3.Lerp(positionOffSet, new Vector3(positionOffSet.x, -Mathf.Abs(initialOffSet.y), positionOffSet.z), Time.deltaTime * (horizontal + 2)); // Lerp for new offset and make it faster if player is moving
                    break;
                case "down":
                    positionOffSet = Vector3.Lerp(positionOffSet, new Vector3(positionOffSet.x, Mathf.Abs(initialOffSet.y), positionOffSet.z), Time.deltaTime * (vertical + 2));
                    break;
            }
        }

        if (positionOffSet.x != 0)  // If the horizontal offSet is 0 don't make extra computations.
        {
            var horizontalPos = limitPlane.GetComponent<PlayerLimitManager>().GetPlayerLocationInPlane(DivideType.Left_Right);
            switch (horizontalPos)
            {
                case "left":
                    positionOffSet = Vector3.Lerp(positionOffSet, new Vector3(Mathf.Abs(initialOffSet.x), positionOffSet.y, positionOffSet.z), Time.deltaTime * (horizontal + 2));
                    break;
                case "right":
                    positionOffSet = Vector3.Lerp(positionOffSet, new Vector3(-Mathf.Abs(initialOffSet.x), positionOffSet.y, positionOffSet.z), Time.deltaTime * (horizontal + 2));
                    break;
            }
        }            
    }
}
