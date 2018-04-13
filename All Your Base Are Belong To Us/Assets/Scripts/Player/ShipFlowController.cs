using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFlowController : MonoBehaviour {

    public float speed = 5.0f;
    public bool flowActive = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(flowActive)
            transform.position += transform.forward * speed * Time.deltaTime;
	}

    public void StopFlow()
    {
        speed = 0.0f;
        flowActive = false;
    }

    public void ActivateFlow(float s)
    {
        speed = s;
        flowActive = true;
    }

}
