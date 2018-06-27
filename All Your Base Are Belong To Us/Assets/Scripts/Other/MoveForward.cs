using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script which only makes the GameObject attached to move forward.
/// </summary>
public class MoveForward : MonoBehaviour {

    public float speed = 1.0f;  // Speed at which the gameObject will move forward.
	
	void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
