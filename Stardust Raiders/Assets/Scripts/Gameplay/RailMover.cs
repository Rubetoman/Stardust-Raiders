using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum to choose mode for the speed of the advance on the Rail.
/// </summary>
public enum SpeedMode
{
    ConstantSpeed,          // Advance at a constant speed.
    TimeBetweenSegments,    // Reach next node on a fixed time, speed is adjusted so it can reach it on time.
}

public class RailMover : MonoBehaviour {

    public Rail rail;
    public PlayMode playMode;                   // Choose between Catmull-Rom or Linear splines for the path.
    public SpeedMode speedMode;                 // Choose between moving at constant speed or in a fixed time for each segment.
    public OrientationMode orientationMode;     // Choose mode of orientation of the object on the rail.
    public float speed = 2.5f;                  // Speed to move (only if ConstantSpeed is selected on SpeedMode).
    public float timeBetweenSegments = 2.5f;    // Time to reach next node (only if TimeBetweenSegments is selected on SpeedMode).
    public bool isReversed;                     // If true it reverses the path.
    public bool isLooping;                      // If true it loops the path when reaching the end node.
    public bool pingPong;                       // If true when the end node is reached it reverses the path.
    public bool loopNode = false;               // If true will loop on the same node that is currently in.

    private int currentSeg;                     // Segment it is currently in.
    private float transition;                   // Point of the segment that is currently in, 0 being at the very start and 1 being the end of the segment.
    private bool isCompleted;                   // If true the end of the Rail has been reached.

    private void FixedUpdate()
    {
        // If there is not rail exit.
        if (!rail)
            return;

        //Avoid using Catmull when less than 4 nodes exist.
        if (rail.nodes.Length < 4 && playMode.ToString().Equals("Catmull"))
        {
            Debug.LogWarning("Not enought nodes for Catmull (minimun of 4 nodes is required, switching to Linear)");
            playMode = PlayMode.Linear;
        }

        //Play the advance if it is not completed.
        if (!isCompleted)
            Play(!isReversed);
    }

    /// <summary>
    /// Plays the movement to follow the path.
    /// </summary>
    /// <param name="forward"> A boolean to make it go forward or backwards.</param>
    private void Play(bool forward = true)
    {
        switch (speedMode)                          // Choose between using speed or time
        {
            default:

            case SpeedMode.ConstantSpeed:           // Make it to move at a constant speed on every node
                float m = (rail.nodes[currentSeg + 1].position - rail.nodes[currentSeg].position).magnitude;
                float s = (Time.deltaTime * 1 / m) * speed;
                transition += (forward) ? s : -s;
                break;

            case SpeedMode.TimeBetweenSegments:     // Make the object to reach next node in timeBetweenSegments
                float move = Time.deltaTime * 1 / timeBetweenSegments;
                transition += (forward) ? move : -move;
                break;
        }
       if (transition > 1 && !loopNode)             // Already reached next segment and it doesn't want to loop over same node.
        {
            transition = 0;                         // Restart the count.
            currentSeg++;                           // Update the segment it is in.
            if(currentSeg == rail.nodes.Length - 1) // Last segment reached.
            {
                if (isLooping)                      // If it is looping start again or play it backwards (pingPong).
                {
                    if (pingPong)
                    {
                        transition = 1;
                        currentSeg = rail.nodes.Length - 2;
                        isReversed = !isReversed;
                    }
                    else
                    {
                        currentSeg = 0;
                    }
                }
                else
                {
                    isCompleted = true;
                }
            }
        }
        else if (transition > 1 && loopNode)    // Already reached next segment and want to loop over same node.
        {
            transition = 0;                     // Restart the count.
        }
        else if(transition < 0 && !loopNode)    // Same logic that before but backwards, reached next segment and it doesn't want to loop over same node.
        {
            transition = 1;                     // Restart the count.
            currentSeg--;                       // Update the segment it is in.
            if (currentSeg == - 1)              // Last segment reached.
            {
                if (isLooping)                  // If it is looping start again or play it backwards (pingPong).
                {
                    if (pingPong)
                    {
                        transition = 0;
                        currentSeg = 0;
                        isReversed = !isReversed;
                    }
                    else
                    {
                        currentSeg = rail.nodes.Length -2;
                    }
                }
            }
        }
        else if (transition < 0 && loopNode) // Same logic that before but backwards, reached next segment and it wants to loop over same node.
        {
            transition = 1;                 // Restart the count
        }
       // Update position and orientation.
        if(playMode != PlayMode.Catmull || currentSeg < rail.nodes.Length - 1)
            transform.position = rail.PositionOnRail(currentSeg, transition, playMode);
        if(orientationMode != OrientationMode.None)
            transform.rotation = rail.OrientationOnRail(currentSeg, transition, orientationMode, transform, isReversed);
    }

    /// <summary>
    /// Returns current segment number.
    /// </summary>
    /// <returns> Int of the current segment.</returns>
    public int GetCurrentSegment()
    {
        return currentSeg;
    }

    /// <summary>
    /// Returns current node GameObject.
    /// </summary>
    /// <returns> Game Object of the current node.</returns>
    public GameObject GetCurrentNode()
    {
        return rail.nodes[currentSeg].gameObject;
    }

    /// <summary>
    /// Get point of the segment that is currently in as a float.
    /// 0 being at the very start and 1 being the end of the segment.
    /// </summary>
    /// <returns></returns>
    public float GetPositionOnSegment()
    {
        return transition;
    }
}
