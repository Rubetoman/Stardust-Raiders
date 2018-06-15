using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour {

    private Quaternion iniRot;
 
    void Start()
    {
        iniRot = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = iniRot;
    }
}
