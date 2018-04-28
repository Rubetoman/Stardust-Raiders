using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickColorController : MonoBehaviour {

    public Material materialA; //First material to change between
    public Material materialB; //Second material to change between
    public float frequency = 0.1f; //Frequency of change between materials
    private float timer = 0.0f;
    private float count = 0.0f;


	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > frequency)
        {
            //If count is even change to materialB else to materialA
            if (count%2 == 0) 
                GetComponent<Renderer>().material = materialB;
            else
                GetComponent<Renderer>().material = materialA;
            timer = 0.0f;
            count += 1;
        }
	}
}
