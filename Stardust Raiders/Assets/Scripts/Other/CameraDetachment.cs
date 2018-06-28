using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that unparents any camera GameObject.
/// </summary>
public class CameraDetachment : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        var camera = other.GetComponent<Camera>();
        if (camera != null)
        {
            camera.transform.parent = null;
            // In case the camera had CameraController script; disable it to avoid bugs.
            if (camera.GetComponent<CameraController>() != null)
                camera.GetComponent<CameraController>().enabled = false;
        }          
    }
}
