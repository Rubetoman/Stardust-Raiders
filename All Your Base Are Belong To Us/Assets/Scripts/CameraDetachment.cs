using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetachment : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        var camera = other.GetComponent<Camera>();
        if (camera != null)
        {
            camera.transform.parent = null;
            if (camera.GetComponent<CameraController>() != null)
                camera.GetComponent<CameraController>().enabled = false;
        }          
    }
}
