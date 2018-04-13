using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier : MonoBehaviour {

    public bool stopFlow = false;
    public float newSpeed = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        var flow = hit.GetComponent<ShipFlowController>();
        if (flow != null)
        {
            if (stopFlow)
                flow.StopFlow();
            else
                flow.ActivateFlow(newSpeed);
        }
    }
}
