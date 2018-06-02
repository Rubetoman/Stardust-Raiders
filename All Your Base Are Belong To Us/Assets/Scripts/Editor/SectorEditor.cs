using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Sector))]
public class SectorEditor : Editor {


    public override void OnInspectorGUI()
    {
        Sector sector = (Sector)target;
        sector.startNode = (GameObject)EditorGUILayout.ObjectField("Start Node", sector.startNode, typeof(GameObject), true);
        sector.speed = EditorGUILayout.FloatField("Speed", sector.speed);
        sector.cameraToggle = EditorGUILayout.Toggle("Change Camera?", sector.cameraToggle);
        if (sector.cameraToggle)
        {
            sector.cameraChange = (Camera)EditorGUILayout.ObjectField("Camera", sector.cameraChange, typeof(Camera), true);
        }
        GUILayout.Label("");
        sector.playerMovement = EditorGUILayout.Toggle("Player Movement", sector.playerMovement);
        sector.pathSelectionToggle = EditorGUILayout.Toggle("Path Selection?", sector.pathSelectionToggle);
        if (sector.pathSelectionToggle)
        {
            EditorGUILayout.HelpBox("-Alternative Rail: The down or right path must be the ones inside the alternative rail. This alternative rail must contain the same nodes as the main rail for it to work properly\n", MessageType.Warning, true);
            sector.alternativeRail = (Rail)EditorGUILayout.ObjectField("Alternative Rail", sector.alternativeRail, typeof(Rail), true);
            sector.divideType = (DivideType)EditorGUILayout.EnumPopup("Divdide Type", sector.divideType);
        }
        GUILayout.Label("");
        sector.railOrientation = (OrientationMode)EditorGUILayout.EnumPopup("Rail Orientation", sector.railOrientation);
        sector.changeScene = EditorGUILayout.Toggle("Load Next Scene", sector.changeScene);
        if (sector.changeScene)
        {
            sector.sceneToLoad = EditorGUILayout.TextField("Scene To Load", sector.sceneToLoad);
        }
        sector.loopSector = EditorGUILayout.Toggle("Loop  Sector", sector.loopSector);
        sector.showEnemyShieldbar = EditorGUILayout.Toggle("Enemy Shieldbar", sector.showEnemyShieldbar);
    }
}
