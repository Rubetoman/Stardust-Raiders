using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to add points to score, when the Player touchs the collider of the GameObject that contains the script.
/// </summary>
public class AddToScoreOnTriggerEnter : MonoBehaviour {

    public int points = 100;    // Points to add to score.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))             // Look for Player colliding.
        {
            GameManager.Instance.AddToTotalScore(points);   // Add points to score.
            AudioManager.Instance.Play("Bonus");            // Play sound effect.
        }
    }
}
