using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : KeepRotating {
    private bool isReversed = false;
	// Use this for initialization
	void Start () {
        if (gameObject.GetComponent<RailMover>() == null)
        {
            print("RailMover script is not attached to the gameObject");
            this.enabled = false;
        }


    }
	
	// Update is called once per frame
	new void Update () {
        isReversed = gameObject.GetComponent<RailMover>().isReversed;
        if (!isReversed)
            base.Update();
        else
            transform.Rotate(-axis, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime);
    }
}
