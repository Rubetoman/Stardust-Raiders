using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to keep any GameObject rotating over an axis.
/// </summary>
public class KeepRotating : MonoBehaviour {

    public float rotationSpeed = 30.0f;     // Speed of the rotation.
    public Vector3 axis = Vector3.forward;  // Axis to rotate around.
	
	protected void Update () {
        transform.Rotate(axis, rotationSpeed * Mathf.Deg2Rad* Time.deltaTime);
	}
}
