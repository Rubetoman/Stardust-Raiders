using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleEditorWindow : EditorWindow {
    // Shield
    const int maxShield = 100;
    int currentShield = maxShield;
    bool godMode = false;
    // Hit Effect
    float recoverTime = 2f;
    Color hitColor;
    int flickCount = 5;
    float flickRate = 0.1f;
    // Destroy Effect
    float destroyTime = 1.0f;

    // Obstacle Controller
    int damage = 10;

    [MenuItem("Window/ObstacleEditor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Obstacle Shield", EditorStyles.boldLabel);
        currentShield = EditorGUILayout.IntField("currentShield", currentShield);
        godMode = EditorGUILayout.Toggle("godMode", godMode);

        GUILayout.Label("Hit Effect", EditorStyles.boldLabel);
        recoverTime = EditorGUILayout.FloatField("recoverTime", recoverTime);
        hitColor = EditorGUILayout.ColorField("hitColor", hitColor);
        flickCount = EditorGUILayout.IntField("flickCount", flickCount);
        flickRate = EditorGUILayout.FloatField("flickRate", flickRate);

        GUILayout.Label("Destroy Effect",EditorStyles.boldLabel);
        destroyTime = EditorGUILayout.FloatField("destroyTime", destroyTime);

        if (GUILayout.Button("Apply Shield Variables"))
        {
            ChangeShieldValues();
        }

        GUILayout.Label("Obstacle Controller", EditorStyles.boldLabel);
        damage = EditorGUILayout.IntField("damage", damage);

        if (GUILayout.Button("Apply Obstacle Variables"))
        {
            ChangeObstacleValues();
        }
    }

    void ChangeShieldValues()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            var obsShield = obj.GetComponent<ObstacleShieldManager>();
            obsShield.currentShield = currentShield;
            obsShield.godMode = godMode;
            obsShield.recoverTime = recoverTime;
            obsShield.hitColor = hitColor;
            obsShield.flickCount = flickCount;
            obsShield.flickRate = flickRate;
            obsShield.destroyTime = destroyTime;
        }
    }

    void ChangeObstacleValues()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            var obs = obj.GetComponent<ObstacleController>();
            if (obs == null)
            {
                foreach(ObstacleController obsController in obj.GetComponentsInChildren<ObstacleController>())
                {
                    obsController.damage = damage;
                }
            }
            else
            {
                obs.damage = damage;
            }
        }
    }
}
