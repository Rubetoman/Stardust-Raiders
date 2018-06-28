using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that makes any GameObject to be scalated over time.
/// </summary>
public class ScaleOverTime : MonoBehaviour {

    public Vector3 goalScale = Vector3.one; // New scale to aim towards.
    public float time = 60.0f;              // Time it will take to reach goalScale.
    public bool loop = true;                // If true it will keep scaling between initialScale and goalScale.
    private Vector3 initialScale;           

	void Start () {
        initialScale = transform.localScale;    // Take initial scale.
        StartCoroutine("ScaleCoroutine");       // Start scaling.
	}

    /// <summary>
    /// IEnumerator that will make the gameObject initialScale scale towards goalScale in the time stated.
    /// </summary>
    IEnumerator ScaleCoroutine()
    {
        float t = 0.0f;
        // Scale over time.
        while(t < time)
        {
            transform.localScale = Vector3.Lerp(initialScale, goalScale, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = goalScale;
        if (loop)
            StartCoroutine("InverseScaleCoroutine");
    }

    /// <summary>
    /// IEnumerator that will make the gameObject goalScale scale towards initialScale in the time stated.
    /// </summary>
    IEnumerator InverseScaleCoroutine()
    {
        float t = 0.0f;
        // Scale over time.
        while (t < time)
        {
            transform.localScale = Vector3.Lerp(goalScale, initialScale, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = goalScale;
        if (loop)
            StartCoroutine("ScaleCoroutine");
    }
}
