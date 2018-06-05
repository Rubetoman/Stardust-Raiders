using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

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
    private Vector3 goalScale = Vector3.one;
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
        goalScale = EditorGUILayout.Vector3Field("goalScale", goalScale);

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
            if(obsShield != null)
            {
                obsShield.currentShield = currentShield;
                obsShield.godMode = godMode;
                obsShield.recoverTime = recoverTime;
                obsShield.hitColor = hitColor;
                obsShield.flickCount = flickCount;
                obsShield.flickRate = flickRate;
                obsShield.destroyTime = destroyTime;
                obsShield.goalScale = goalScale;
                EditorUtility.SetDirty(obsShield);
            }
            else
            {
                var childs = obj.GetComponentsInChildren<ObstacleShieldManager>();
                foreach (ObstacleShieldManager childShield in childs)
                {
                    childShield.currentShield = currentShield;
                    childShield.godMode = godMode;
                    childShield.recoverTime = recoverTime;
                    childShield.hitColor = hitColor;
                    childShield.flickCount = flickCount;
                    childShield.flickRate = flickRate;
                    childShield.destroyTime = destroyTime;
                    childShield.goalScale = goalScale;
                    EditorUtility.SetDirty(childShield);
                }
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
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
                    EditorUtility.SetDirty(obsController);
                }
            }
            else
            {
                obs.damage = damage;
                EditorUtility.SetDirty(obj);
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}
