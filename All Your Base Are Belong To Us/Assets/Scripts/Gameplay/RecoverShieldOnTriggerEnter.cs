using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to recover the shield of the Player, when the Player touchs the collider of the GameObject that contains the script.
/// </summary>
public class RecoverShieldOnTriggerEnter : MonoBehaviour {

    public int shieldToRecover = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))                     // Look for Player colliding.
        {
            var hit = other.gameObject;
            var shield = hit.GetComponent<PlayerShieldManager>();   // Take Player shield.
            if (shield != null)
            {
                shield.RecoverShield(shieldToRecover);
            }
        }
    }
}
