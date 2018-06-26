using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

/// <summary>
/// Class for the Obstacle Editor Window. This window can edit multiple obstacle variables at the same time.
/// Changes variables of ObstacleShieldManager and ObstacleController for every GameObject or child selected on the Unity Editor.
/// </summary>
public class ObstacleEditorWindow : EditorWindow {
    // Obstacle Shield
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
        GUILayout.Label("Obstacle Shield", EditorStyles.boldLabel);                 // Show Label
        currentShield = EditorGUILayout.IntField("currentShield", currentShield);   // Show Int field
        godMode = EditorGUILayout.Toggle("godMode", godMode);                       // Show toggle for Bool

        GUILayout.Label("Hit Effect", EditorStyles.boldLabel);
        recoverTime = EditorGUILayout.FloatField("recoverTime", recoverTime);       // Show Float field
        hitColor = EditorGUILayout.ColorField("hitColor", hitColor);                // Show color field
        flickCount = EditorGUILayout.IntField("flickCount", flickCount);            
        flickRate = EditorGUILayout.FloatField("flickRate", flickRate);             

        GUILayout.Label("Destroy Effect",EditorStyles.boldLabel);
        destroyTime = EditorGUILayout.FloatField("destroyTime", destroyTime);
        goalScale = EditorGUILayout.Vector3Field("goalScale", goalScale);           // Show Vector3 field

        if (GUILayout.Button("Apply Shield Variables"))                             // Show button and onClick action
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

    /// <summary>
    /// This function searches through all selected GameObjects and their child for ObstacleShieldManager script. Once the script has been found the variables will be updated to the ones showed on the Window.
    /// </summary>
    void ChangeShieldValues()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Check if the selected GameObject contains the ObstacleShieldManager.
            var obsShield = obj.GetComponent<ObstacleShieldManager>();
            if (obsShield != null)                                           
            {
                obsShield.currentShield = currentShield;
                obsShield.godMode = godMode;
                obsShield.recoverTime = recoverTime;
                obsShield.hitColor = hitColor;
                obsShield.flickCount = flickCount;
                obsShield.flickRate = flickRate;
                obsShield.destroyTime = destroyTime;
                obsShield.goalScale = goalScale;

                #if UNITY_STANDALONE && UNITY_EDITOR        // Only run when in Unity Editor.
                    if (!Application.isPlaying)             // Avoid seting it dirty when in play mode.                     
                        EditorUtility.SetDirty(obsShield);  // Set GameObject dirty after the changes are made.
                #endif
            }
            else
            {
                var childs = obj.GetComponentsInChildren<ObstacleShieldManager>();
                // Iterate trought all childs if the selected GameObject didn't contain the script.
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

                    #if UNITY_STANDALONE && UNITY_EDITOR            // Only run when in Unity Editor.
                        if (!Application.isPlaying)                 // Avoid setting dirty when in play mode.
                            EditorUtility.SetDirty(childShield);    // Set GameObject dirty after the changes are made.
                    #endif
                }
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());   // Set scene dirty after the changes are made so they can be saved.
    }

    /// <summary>
    /// This function searches through all selected GameObjects and their child for ObstacleController script. Once the script has been found the variables will be updated to the ones showed on the Window.
    /// </summary>
    void ChangeObstacleValues()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Check if the selected GameObject contains the ObstacleShieldManager.
            var obs = obj.GetComponent<ObstacleController>();
            if (obs != null)
            {
                obs.damage = damage;
                #if UNITY_STANDALONE && UNITY_EDITOR        // Only run when in Unity Editor.
                    if (!Application.isPlaying)             // Avoid setting dirty when in play mode.
                        EditorUtility.SetDirty(obj);        // Set GameObject dirty after the changes are made so they can be saved.
                #endif
            }
            else
            {
                foreach (ObstacleController obsController in obj.GetComponentsInChildren<ObstacleController>()) // Iterate trought all childs if the selected GameObject didn't contain the script.
                {
                    obsController.damage = damage;
                    #if UNITY_STANDALONE && UNITY_EDITOR            // Only run when in Unity Editor.
                        if (!Application.isPlaying)                 // Avoid setting dirty when in play mode.
                            EditorUtility.SetDirty(obsController);  // Set GameObject dirty after the changes are made so they can be saved.
                    #endif
                }
                                               
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());   // Set scene dirty after the changes are made so they can be saved.
    }
}
