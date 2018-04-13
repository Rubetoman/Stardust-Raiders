using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour {

    public Rail rail;
    public float timeBetweenSegments = 2.5f;

    private int currentSeg;
    private float transition;
    private bool isCompleted;

    private void Update()
    {
        if (!rail)
            return;

        if (!isCompleted)
            Play();
    }

    private void Play()
    {
        transition += Time.deltaTime * 1 / timeBetweenSegments;
        if(transition > 1)
        {
            transition = 0;
            currentSeg++;
        }
        else if(transition < 0)
        {
            transition = 1;
            currentSeg--;
        }
        transform.position = rail.LinearPosition(currentSeg, transition);
        transform.rotation = rail.Orientation(currentSeg, transition);
    }
}
