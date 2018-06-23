using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour {

    public bool x, y, z;

    private Quaternion initialRotation;
    private Quaternion newRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        newRotation.x = x ? initialRotation.x : transform.localRotation.x;
        newRotation.y = y ? initialRotation.y : transform.localRotation.y;
        newRotation.z = z ? initialRotation.z : transform.localRotation.z;
        transform.rotation = newRotation;
    }
}
