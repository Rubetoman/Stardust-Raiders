using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll Wheel obstacle. Inherits from KeepRotating.
/// </summary>
public class WheelController : KeepRotating {

    private bool isReversed = false;    // Know if rotate the wheel reversed. It is true when the RailMover attached is traversing the rail backwards.

	void Start () {
        // Look out for the RailMover, if not disable the script.
        if (gameObject.GetComponent<RailMover>() == null)
        {
            print("RailMover script is not attached to the gameObject");
            this.enabled = false;
        }
    }
	
	new void Update () {
        isReversed = gameObject.GetComponent<RailMover>().isReversed;                   // Look if is moving backwards on the rail.
        if (!isReversed)
            base.Update();                                                              // Use the normal rotation.
        else
            transform.Rotate(-axis, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime);    // Rotate in the opposite direction.
    }
}
