using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverShieldOnTriggerEnter : MonoBehaviour {

    public int shieldToRecover = 50;

    private void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        var shield = hit.GetComponent<PlayerShieldManager>();
        if (shield != null && !shield.damaged)
        {
            shield.RecoverShield(shieldToRecover);
        }
    }
}
