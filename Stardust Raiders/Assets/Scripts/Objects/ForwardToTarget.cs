using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any gameObject that contains this script will be forwarded to the target. WARNING: Only works with targets that have a parent.
/// </summary>
public class ForwardToTarget : MonoBehaviour {
    public Transform target;                    // Target the gameObject will forward to
    public float startTime = 5.0f;              // Delay time for the start of the forwarding
    public float forwardTime = 1.0f;            // Time it will take to reach the target
    private bool finished = false;              // If the forwarding has already been completed

	void Start () {
        StartCoroutine("ForwardAnimation");
	}
	
	void FixedUpdate () {
        // Forwarding completed, then follow the target.
        if (finished)
            transform.localPosition = target.localPosition;
	}

    /// <summary>
    /// Animation to make the forwarding smooth.
    /// </summary>
    IEnumerator ForwardAnimation()
    {
        // Wait for the time specified 
        yield return new WaitForSeconds(startTime);
        float t = 0.0f;
        transform.parent = target.parent;               // Make the gameObject children of the same parent as the target.
        var currentPosition = transform.localPosition;
        // Forward to the target smoothly on the time given.
        while (t < forwardTime)
        {
            t += Time.fixedDeltaTime;
            transform.localPosition = Vector3.Lerp(currentPosition, target.localPosition, t / forwardTime);
            yield return null;
        }
        finished = true;
    }
}
