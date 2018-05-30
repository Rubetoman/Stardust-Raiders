using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject mainMenuUI;

    private  GameObject myEventSystem;
    private GameObject lastSelected;
    private void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");
        if(myEventSystem != null)
            lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;
    }
    void Update()
    {
        if (Input.GetButtonDown("Back"))
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.Options)
            {
                GetComponentInChildren<OptionsMenu>().GoBack();
                mainMenuUI.SetActive(true);
                lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
            }
            else if(GameManager.Instance.GetGameState() == GameManager.StateType.MainMenu)
            {
                // Hide Main Menu and show tittle screen
            }
        }

        // If no UI gameObject was selected and horizontal or vertical Input is detected, select the last selected button.
        if (myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
        }
        else
        {
            // If a UI gameObject is selected and mouse Input is detected, deselect it.
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                lastSelected = myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
            }
        }
    }
    public void PlayGame()
    {
        GameManager.Instance.SetGameState(GameManager.StateType.Play);
        GameManager.Instance.NextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
