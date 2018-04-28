using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleShieldManager : ShieldManager {
    [Header("Destroy Effect")]
    public Transform explosionSpawnPoint;
    public Vector3 goalScale = Vector3.zero;
    public float time = 60.0f;
    private Vector3 initialScale;

    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Destroys the GameObject
    /// If it has a parent it will destroy the whole GameObject hierarchy if not only the objetc the script is attached to.
    /// </summary>
    protected override void Die()
    {
        initialScale = transform.localScale;
        StartCoroutine("DestroyEffect", 0f);
    }

    IEnumerator DestroyEffect()
    {
        float t = 0.0f;
        float t2 = 0.5f;

        while (t < time)
        {
            if (t2 > 0.5f)
            {
                Destroy(Instantiate(deathEffect, explosionSpawnPoint.position, Quaternion.identity), 1.0f);
                t2 = 0.0f;
            }
            transform.localScale = Vector3.Lerp(initialScale, goalScale, t / time);
            t += Time.deltaTime;
            t2 += Time.deltaTime;
            yield return null;
        }
        transform.localScale = goalScale;
        Destroy(gameObject);
    }
}
