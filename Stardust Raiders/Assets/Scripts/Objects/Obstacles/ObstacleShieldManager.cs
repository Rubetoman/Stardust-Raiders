using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shield manager for all the obstacles with shield. Inherits from ShieldManager.
/// </summary>
public class ObstacleShieldManager : ShieldManager {

    [Header("Destroy Effect")]
    public Transform explosionSpawnPoint;       // Point in which the explosion GameObject will be spawned.
    public Vector3 goalScale = Vector3.zero;    // New scale to change when shield has reached 0.
    public float destroyTime = 1.0f;            // Time for the destroy animation.
    public int destroyScore = 10;               // Score to give the Player when the obstacle is destroyed.
    private Vector3 initialScale;               // Initial scale of the obstacle.

    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Destroys the GameObject.
    /// </summary>
    protected override void Die()
    {
        initialScale = transform.localScale;
        StartCoroutine("DestroyEffect", 0f);
        GameManager.Instance.AddToTotalScore(destroyScore); // Add points to score.
    }

    /// <summary>
    /// Animation for the destruction of the obstacle:
    /// Changes the scale while spawning explosions and at the end the obstacle is completely destroyed.
    /// </summary>
    IEnumerator DestroyEffect()
    {
        float t = 0.0f;
        float t2 = 0.5f;
        // Make smooth change to new scale while spawning explosions.
        while (t < destroyTime)
        {
            if (t2 > 0.5f)
            {
                Destroy(Instantiate(deathEffect, explosionSpawnPoint.position, Quaternion.identity), 1.0f);
                t2 = 0.0f;
            }
            transform.localScale = Vector3.Lerp(initialScale, goalScale, t / destroyTime);
            t += Time.deltaTime;
            t2 += Time.deltaTime;
            yield return null;
        }
        transform.localScale = goalScale;
        Destroy(gameObject);
    }
}
