using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObstacleGroupManager : MonoBehaviour {

    public Transform[] obstacles;
    [Space(10)]
    [Header("Shield")]
    public const int maxShield = 100;
    public int currentShield = maxShield;
    public bool godMode = false;
    [Header("Hit Effect")]
    public float recoverTime = 2f;
    public Color hitColor;
    public int flickCount = 5;
    public float flickRate = 0.1f;
    [Header("Destroy Effect")]
    public float time = 60.0f;

    private void Start()
    {
        gameObject.GetComponent<ObstacleGroupManager>().enabled = false;
    }

    // Update is called once per frame
    void LateUpdate () {
        obstacles = GetComponentsInChildren<Transform>();

        foreach (Transform o in obstacles)
        {
            if (o.GetComponent<ObstacleShieldManager>() != null)
            {
                var obsShield = o.GetComponent<ShieldManager>();
                obsShield.currentShield = currentShield;
                obsShield.godMode = godMode;
                obsShield.recoverTime = recoverTime;
                obsShield.hitColor = hitColor;
                obsShield.flickCount = flickCount;
                obsShield.flickRate = flickRate;
            }
            if (o.GetComponent<ObstacleShieldManager>() != null)
                o.GetComponent<ObstacleShieldManager>().destroyTime = time;
        }
    }
}
