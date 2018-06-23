using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuildVersion : MonoBehaviour {

	void Start () {
        if (gameObject.GetComponent<Text>() != null)
            gameObject.GetComponent<Text>().text = "V. " + Application.version;
        else
            Debug.LogError("The gameObject doesn't have a Text component");
	}
}
