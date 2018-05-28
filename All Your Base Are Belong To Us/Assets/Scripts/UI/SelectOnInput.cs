using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {
    public GameObject firstSelection;

    private GameObject myEventSystem;
    private GameObject lastSelected;
    // Use this for initialization
    void Awake () {
        myEventSystem = GameObject.Find("EventSystem"); // Set scene EventSystem.
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        lastSelected = firstSelection;     // Use the GameObject passed as selected by the EventSystem.
        if (myEventSystem != null)
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
    }

    private void OnDisable()
    {
    // Once disabled use the first button of the previous menu as selected.
    if (myEventSystem != null)
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject);
    }
}
