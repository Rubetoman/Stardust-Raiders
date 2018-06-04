using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(Sector))]
public class SectorEditor : Editor {


    public override void OnInspectorGUI()
    {
        Sector sector = (Sector)target;
        sector.startNode = (GameObject)EditorGUILayout.ObjectField("Start Node", sector.startNode, typeof(GameObject), true);
        sector.speed = EditorGUILayout.FloatField("Speed", sector.speed);
        sector.cameraChange = EditorGUILayout.Toggle("Change Camera?", sector.cameraChange);
        if (sector.cameraChange)
        {
            sector.newCamera = (Camera)EditorGUILayout.ObjectField("Camera", sector.newCamera, typeof(Camera), true);
        }
        GUILayout.Label("");
        sector.playerMovement = EditorGUILayout.Toggle("Player Movement", sector.playerMovement);
        sector.railOrientation = (OrientationMode)EditorGUILayout.EnumPopup("Rail Orientation", sector.railOrientation);
        sector.railMode = (PlayMode)EditorGUILayout.EnumPopup("Rail Mode", sector.railMode);
        if(sector.railMode != PlayMode.Catmull)
        {
            sector.pathSelection = EditorGUILayout.Toggle("Path Selection?", sector.pathSelection);
            if (sector.pathSelection)
            {
                EditorGUILayout.HelpBox("The down or right path must be the ones inside the alternative rail.", MessageType.Warning, true);
                sector.alternativeRail = (Rail)EditorGUILayout.ObjectField("Alternative Rail", sector.alternativeRail, typeof(Rail), true);
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
            sector.sceneToLoad = EditorGUILayout.TextField("Scene To Load", sector.sceneToLoad);
        }
        sector.loopSector = EditorGUILayout.Toggle("Loop  Sector", sector.loopSector);
        sector.showEnemyShieldbar = EditorGUILayout.Toggle("Enemy Shieldbar", sector.showEnemyShieldbar);
        sector.changeMusic = EditorGUILayout.Toggle("Change music?", sector.changeMusic);
        if (sector.changeMusic)
        {
            sector.musicClipName = EditorGUILayout.TextField("Clip Name", sector.musicClipName);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(sector);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
