using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that changes the Text attached to the GameObject containing this script for the build version.
/// </summary>
public class ShowBuildVersion : MonoBehaviour {

	void Start () {
        if (gameObject.GetComponent<Text>() != null)
            gameObject.GetComponent<Text>().text = "V. " + Application.version;
        else
            Debug.LogError("The gameObject doesn't have a Text component");
	}
}
