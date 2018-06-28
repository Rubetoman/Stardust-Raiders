using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Custom Editor for the Sector Class.
/// </summary>
[CustomEditor(typeof(Sector))]
public class SectorEditor : Editor {

    public override void OnInspectorGUI()
    {
        Sector sector = (Sector)target; // Targeted class.
        sector.startNode = (GameObject)EditorGUILayout.ObjectField("Start Node", sector.startNode, typeof(GameObject), true);   // Show GameObject field.
        sector.speed = EditorGUILayout.FloatField("Speed", sector.speed);                                                       // Show Float field.
        sector.cameraChange = EditorGUILayout.Toggle("Change Camera?", sector.cameraChange);                                    // Show toggle for Bool.
        if (sector.cameraChange)                                                                                                // If previous bool was true show extra field.
        {
            sector.newCamera = (Camera)EditorGUILayout.ObjectField("Camera", sector.newCamera, typeof(Camera), true);           // Show Camera field.
        }
        GUILayout.Label("");
        sector.playerMovement = EditorGUILayout.Toggle("Player Movement", sector.playerMovement);
        sector.railOrientation = (OrientationMode)EditorGUILayout.EnumPopup("Rail Orientation", sector.railOrientation);        // Show enumerator field.
        sector.railMode = (PlayMode)EditorGUILayout.EnumPopup("Rail Mode", sector.railMode);
        if(sector.railMode != PlayMode.Catmull)                                                                                 // If previous enum was set to the specified value show extra field.
        {
            sector.pathSelection = EditorGUILayout.Toggle("Path Selection?", sector.pathSelection);                             
            if (sector.pathSelection)
            {
                EditorGUILayout.HelpBox("The down or right path must be the ones inside the alternative rail.", MessageType.Warning, true); // Show warning message.
                sector.alternativeRail = (Rail)EditorGUILayout.ObjectField("Alternative Rail", sector.alternativeRail, typeof(Rail), true); // Show Rail field.
                sector.divideType = (DivideType)EditorGUILayout.EnumPopup("Divdide Type", sector.divideType);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Path Selection can be combined with Catmull rail mode", MessageType.Warning, true);
        }
        GUILayout.Label("");
        sector.changeScene = EditorGUILayout.Toggle("Load a Scene?", sector.changeScene);
        if (sector.changeScene)
        {
            sector.sceneToLoad = EditorGUILayout.TextField("Scene To Load", sector.sceneToLoad);                                // Show Text field.
        }
        sector.loopSector = EditorGUILayout.Toggle("Loop  Sector", sector.loopSector);
        sector.showEnemyShieldbar = EditorGUILayout.Toggle("Enemy Shieldbar", sector.showEnemyShieldbar);
        sector.changeMusic = EditorGUILayout.Toggle("Change music?", sector.changeMusic);
        if (sector.changeMusic)
        {
            sector.musicClipName = EditorGUILayout.TextField("Clip Name", sector.musicClipName);
        }
        #if UNITY_STANDALONE && UNITY_EDITOR                                        // Only run when in Unity Editor.
            if (GUI.changed && !Application.isPlaying)                              // If there was a change on the script GUI and outside play mode.
            {
                EditorUtility.SetDirty(sector);                                     // Set GameObject dirty after the changes are made.
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());   // Set scene dirty after the changes are made so they can be saved.
            }
        #endif
    }
}
