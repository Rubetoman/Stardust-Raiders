using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayMode
{
    Linear,
    Catmull,
}

public enum OrientationMode
{
    Line,
    Nodes,
    None,
}

[ExecuteInEditMode]
public class Rail : MonoBehaviour {
    public Transform[] nodes;   // Transform of the nodes that form the Rail, the gameObject that contains the script followed by all his children will be added automaticaly. 

    private void Start()
    {
        // Add all children Transforms to the nodes array
        nodes = GetComponentsInChildren<Transform>();
    }

    /// <summary>
    /// Calls to LinearPosition() or CatmullPosition() function depending on PlayMode
    /// </summary>
    /// <param name="seg"> Segment we are in</param>
    /// <param name="ratio"> Ratio of the segment we are currently</param>
    /// <param name="mode"> Which mode was selected between Catmull or Linear</param>
    /// <returns>A Vector3 with the position update</returns>
    public Vector3 PositionOnRail(int seg, float ratio, PlayMode mode)
    {
        switch (mode)
        {
            default:
            case PlayMode.Linear:
                return LinearPosition(seg, ratio);
            case PlayMode.Catmull:
                    return CatmullPosition(seg, ratio);
        }

    }

    /// <summary>
    /// Computes the new position over the rail using linear splines
    /// </summary>
    /// <param name="seg"> Segment we are in</param>
    /// <param name="ratio"> Ratio of the segment we are currently</param>
    /// <returns>A Vector3 with the position update</returns>
    public Vector3 LinearPosition(int seg, float ratio)
    {
        Vector3 p1 = nodes[seg].position;
        Vector3 p2 = nodes[seg].position;
        if(nodes.Length > seg+1)
            p2 = nodes[seg+1].position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    /// <summary>
    /// Computes the new position over the rail using Catmull-Rom splines. 
    /// At least 4 nodes are required (3 nodes + the rail itself)
    /// </summary>
    /// <param name="seg"> Segment we are in</param>
    /// <param name="ratio"> Ratio of the segment we are currently</param>
    /// <returns>A Vector3 with the position update</returns>
    public Vector3 CatmullPosition(int seg, float ratio)
    {
        Vector3 p1, p2, p3, p4;         // 4 points of a Catmull-Rom Spline
        if(seg == 0)                    // We are on the first segment
        {
            p1 = nodes[seg].position;   // Previous node doen't exist, we use the first one for the two first points on the spline
            p2 = p1;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }
        else if (seg == nodes.Length-2)   // We are reaching the end
        {
            p1 = nodes[seg-1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = p3;                      // There aren't more nodes, we use the last one for the two last points on the spline
        }
        else //Common case 
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }

        //Catmull-Rom Splines formula https://www.mvps.org/directx/articles/catmull/
        float t2 = ratio * ratio;
        float t3 = t2 * ratio;

        float x = 0.5f * ((2.0f * p2.x) 
            + (-p1.x + p3.x) 
            * ratio + (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x -p4.x) 
            * t2 + (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) 
            * t3);

        float y = 0.5f * ((2.0f * p2.y)
            + (-p1.y + p3.y)
            * ratio + (2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y)
            * t2 + (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y)
            * t3);

        float z = 0.5f * ((2.0f * p2.z)
            + (-p1.z + p3.z)
            * ratio + (2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z)
            * t2 + (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z)
            * t3);

        return new Vector3(x, y, z);
    }
    /// <summary>
    /// Calls to LinearPosition() or CatmullPosition() function depending on PlayMode
    /// </summary>
    /// <param name="seg"> Segment we are in</param>
    /// <param name="ratio"> Ratio of the segment we are currently</param>
    /// <param name="mode"> Which mode was selected between Catmull or Linear</param>
    /// <returns>A Quaternion with the position update</returns>
    public Quaternion OrientationOnRail(int seg, float ratio, OrientationMode mode, Transform trans, bool isReversed)
    {
        switch (mode)
        {
            default:
            case OrientationMode.Line:
                return LineOrientation(seg, ratio, trans, isReversed);
            case OrientationMode.Nodes:
                return NodeOrientation(seg, ratio, isReversed);
        }

    }

   

    /// <summary>
    /// Computes the new orientation over the rail using the lines connecting nodes as a reference.
    /// Will look to the next node.
    /// </summary>
    /// <param name="seg"> Segment we are in</param>
    /// <param name="ratio"> Ratio of the segment we are currently</param>
    /// <param name="trans"> Transform of the object moving on the rail</param>
    /// <param name="isReversed"> Boolean to know if we are going forward or backwards</param>
    /// <returns>A Quaternion with the rotation update</returns>
    public Quaternion LineOrientation(int seg, float ratio, Transform trans, bool isReversed)
    {
        Quaternion q1 = trans.rotation;
        if (!isReversed && nodes.Length > seg + 1)  // Common case
        {
            trans.LookAt(nodes[seg + 1]);
            Quaternion q2 = trans.rotation;
            return Quaternion.Lerp(q1, q2, ratio);
        }
        else // Reached end of the Rail
        {
            trans.LookAt(nodes[seg]);
            Quaternion q2 = trans.rotation;
            return Quaternion.Lerp(q2, q1, ratio);
        }          
    }

    /// <summary>
    /// Computes the new orientation over the rail using the nodes orientation as a reference.
    /// </summary>
    /// <param name="seg"> Segment we are in</param>
    /// <param name="ratio"> Ratio of the segment we are currently</param>
    /// <param name="isReversed"> Boolean to know if we are going forward or backwards</param>
    /// <returns>A Quaternion with the rotation update</returns>
    public Quaternion NodeOrientation(int seg, float ratio, bool isReversed)
    {
        Quaternion q1 = nodes[seg].rotation;
        if (!isReversed && nodes.Length > seg + 1)  // Common case
        {
            Quaternion q2 = nodes[seg + 1].rotation;
            return Quaternion.Lerp(q1, q2, ratio);
        }
        else // Reached end of the Rail
        {
            if (!isReversed)
            {
                Quaternion q2 = nodes[seg - 1].rotation;
                return Quaternion.Lerp(q1, q2, ratio);
            }
            else
            {
                Quaternion q2 = nodes[seg + 1].rotation;
                return Quaternion.Lerp(q2, q1, ratio);
            }
        }

    }

    /// <summary>
    /// Draws the lines on the editor
    /// </summary>
    private void OnDrawGizmos()
    {
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            UnityEditor.Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 3.0f);
        }
    }
}
