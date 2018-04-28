using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingBullet : BulletController {
    [Header("Seeking Configuration")]
    public float speed = 1.0f;
    public float turnSpeed = 30.0f;
    public float startSeeking = 1.0f;
    public float stopSeeking = 4.0f;

    private GameObject player;
    private Quaternion newRotation;
    private float t = 0.0f;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if(t > startSeeking && t < stopSeeking)
        {
            //Find player position
            if (player != null)
                newRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            else
                Debug.LogError("Player couldn't be found");

            //Turn
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        }
        
        //GoForward
        transform.position += transform.forward * speed * Time.deltaTime;
	}
}
