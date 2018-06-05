﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToScoreOnTriggerEnter : MonoBehaviour {

    public int points = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            GameManager.Instance.AddToTotalScore(points);
            AudioManager.Instance.Play("Bonus");
        }
    }
}
