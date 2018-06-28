using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that freezes the rotation on the selected axis.
/// </summary>
public class FreezeRotation : MonoBehaviour {

    public bool x, y, z;    // Axis to freeze: true to freeze them, false otherwise.

    private Quaternion initialRotation;
    private Quaternion newRotation;

    void Start()
    {
        initialRotation = transform.rotation;   // Get start rotation.
    }

    void LateUpdate()
    {
        // Keep same rotation on the selected axis.
        newRotation.x = x ? initialRotation.x : transform.localRotation.x;
        newRotation.y = y ? initialRotation.y : transform.localRotation.y;
        newRotation.z = z ? initialRotation.z : transform.localRotation.z;
        transform.rotation = newRotation;
    }
}
