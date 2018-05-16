using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Sectors:\n " +
            "-Camera: If None previous camera will be used as default\n" +
            "-Alternative Rail: The down or right path must be the ones inside the alternative rail. This alternative rail must contain the same nodes as the main rail for it to work properly\n" +
            "-Divide Type: If Alternative Rail is Null doesn't affect anything", MessageType.Warning, true);
        DrawDefaultInspector();
        /*LevelManager levelManager = (LevelManager)target;
        // Level Manager Variables
        levelManager.gameplayPlane = (GameObject)EditorGUILayout.ObjectField("Gameplay Plane", levelManager.gameplayPlane, typeof(GameObject), true);

        // Game Over Variables
        GUILayout.Label("Path Selection", EditorStyles.boldLabel);
        levelManager.gameOverScreen = (Image)EditorGUILayout.ObjectField("Game Over Screen", levelManager.text, typeof(Image), true);
        levelManager.gameOverText = (Text)EditorGUILayout.ObjectField("Game Over Text", levelManager.gameOverText, typeof(Text), true);
        levelManager.score = (Text)EditorGUILayout.ObjectField("Score Text", levelManager.score, typeof(Text), true);

        //Sectors
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sectors"), true);
        SerializedProperty sp = serializedObject.FindProperty("sectors");
        for (int x = 0; x < sp.arraySize; x++)
        {
            var subObject = sp.GetArrayElementAtIndex(x);

            EditorGUILayout.PropertyField(sp.GetArrayElementAtIndex(x));

        }




        //myScript.flag = GUILayout.Toggle(myScript.flag, "Flag");
        //LevelManager.Sector.divideType = EditorGUILayout.EnumPopup("divideType", dt);
        //if (myScript.flag)
        //   myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);

        // Path Selection
        GUILayout.Label("Path Selection", EditorStyles.boldLabel);
        levelManager.limitPlane = (GameObject)EditorGUILayout.ObjectField("Limit Plane", levelManager.limitPlane, typeof(GameObject), true);
        levelManager.text = (GameObject)EditorGUILayout.ObjectField("Choose Text", levelManager.text, typeof(GameObject), true);
        SerializedProperty arrows = serializedObject.FindProperty("arrows");
        EditorGUILayout.PropertyField(arrows, true);
        levelManager.chooseTime = EditorGUILayout.FloatField("Choose Time", levelManager.chooseTime);
        levelManager.flickFrequency = EditorGUILayout.FloatField("Arrows Flick Freq.", levelManager.flickFrequency);
        */
    }
}
