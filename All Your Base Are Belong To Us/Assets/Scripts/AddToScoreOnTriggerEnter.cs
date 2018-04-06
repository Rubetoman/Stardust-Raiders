using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToScoreOnTriggerEnter : MonoBehaviour {

    public int points = 100;
    public ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        ScoreManager.Instance.score += points;
    }
}
