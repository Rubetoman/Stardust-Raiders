using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

    public int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        var health = hit.GetComponent<HealthController>();
        if (health != null && !health.damaged)
        {
            health.TakeDamage(damage);
        }
    }
}
