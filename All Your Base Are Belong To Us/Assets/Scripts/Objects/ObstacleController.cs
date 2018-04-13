using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

    public int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        var shield = hit.GetComponent<ShieldController>();
        if (shield != null && !shield.damaged)
        {
            shield.TakeDamage(damage);
        }
    }
}
