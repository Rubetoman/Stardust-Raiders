using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToScoreOnTriggerEnter : MonoBehaviour {

    public int points = 100;
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerCollider"))
            ScoreManager.Instance.score += points;
    }
}
