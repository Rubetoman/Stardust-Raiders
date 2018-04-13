using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rail : MonoBehaviour {
    private Transform[] nodes;

    private void Start()
    {
        nodes = GetComponentsInChildren<Transform>();
    }

    public Vector3 LinearPosition(int seg, float ratio)
    {
        Vector3 p1 = nodes[seg].position;
        Vector3 p2 = nodes[seg].position;
        if(nodes[seg+1] != null)
            p2 = nodes[seg+1].position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    public Quaternion Orientation(int seg, float ratio)
    {
        Quaternion q1 = nodes[seg].rotation;
        Quaternion q2 = nodes[seg + 1].rotation;

        return Quaternion.Lerp(q1, q2, ratio);
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            UnityEditor.Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 3.0f);
        }
    }
}
