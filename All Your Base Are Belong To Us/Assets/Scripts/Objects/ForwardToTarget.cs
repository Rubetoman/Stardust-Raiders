using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any gameObject that contains this script will be forwarded to the target. WARNING: Only works with targets that have a parent
/// </summary>
public class ForwardToTarget : MonoBehaviour {
    public Transform target;                    // Target the gameObject will forward to
    public float startTime = 5.0f;              // Delay time for the start of the forwarding
    public float forwardTime = 1.0f;            // Time it will take to reach the target
    //public bool returnToStartPosition = false;  // If the gameObject will return to the start position after completing the forwarding

    //private Transform startPosition;        
    //private Transform startParent;      // The parent of the gameObject before forwarding
    private bool finished = false;      // If the forwarding has already been completed
	// Use this for initialization
	void Start () {
        /*startPosition = gameObject.transform;
        if(transform.parent != null)
            startParent = transform.parent;*/
        StartCoroutine("ForwardAnimation");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Forwarding completed, see if we continue following the target
        if (/*!returnToStartPosition && */finished)
            transform.localPosition = target.localPosition;
	}

    /// <summary>
    /// Animation to make the forwarding smooth
    /// </summary>
    IEnumerator ForwardAnimation()
    {
        // Wait for the time specified 
        yield return new WaitForSeconds(startTime);
        float t = 0.0f;
        transform.parent = target.parent;               // Make the gameObject children of the same parent as the target
        var currentPosition = transform.localPosition;
        // Forward to the target smoothly on the time given
        while (t < forwardTime)
        {
            t += Time.fixedDeltaTime;
            transform.localPosition = Vector3.Lerp(currentPosition, target.localPosition, t / forwardTime);
            yield return null;
        }
        finished = true;
        /*if (returnToStartPosition)
        {
            // Give the parent back if it had a parent before,
            if (startParent != null)
            {
                transform.parent = startParent;
                transform.localPosition = startPosition.localPosition;
            }
            else
            {
                transform.position = startPosition.position;
            }
        }*/
    }
}
