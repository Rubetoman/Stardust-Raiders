using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        var shield = other.gameObject.GetComponent<PlayerShieldManager>();
        if (shield != null)
            shield.TakeDamage(damage);
    }

    private void OnTriggerStay(Collider other)
    {
        var shield = other.gameObject.GetComponent<PlayerShieldManager>();
        if (shield != null)
            shield.TakeDamage(damage);
    }
}
