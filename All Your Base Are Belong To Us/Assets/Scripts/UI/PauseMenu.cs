using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuUI;

    private GameObject myEventSystem;
    private GameObject lastSelected;

    private void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");
        if (myEventSystem != null)
            lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;
    }

    void Update () {
        if (Input.GetButtonDown("Start"))
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.PauseMenu)
            {
                Resume();
            }
            else if(GameManager.Instance.GetGameState() == GameManager.StateType.Play)
            {
                Pause();
            }
        }
        if (Input.GetButtonDown("Back"))
        {
            switch (GameManager.Instance.GetGameState()) {
                case GameManager.StateType.PauseMenu:
                Resume();
                break;
            case GameManager.StateType.Options:
                GetComponentInChildren< OptionsMenu > ().GoBack();
                pauseMenuUI.SetActive(true);
                break;
            case GameManager.StateType.Play:
                Pause();
                break;
            }
            /*
            if (GameManager.Instance.GetGameState() == GameManager.StateType.PauseMenu)
                Resume();
            else if (GameManager.Instance.GetGameState() == GameManager.StateType.Options)
            {
                GetComponentInChildren<OptionsMenu>().GoBack();
                pauseMenuUI.SetActive(true);
            }
            else if(GameManager.Instance.GetGameState() == GameManager.StateType.Play)
                Pause();*/
        }
        if (GameManager.Instance.GetGameState() == GameManager.StateType.PauseMenu)
        {
            // If no UI gameObject was selected and horizontal or vertical Input is detected, select the last selected button.
            if (myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null)
            {
                if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
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
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.SetGameState(GameManager.StateType.Play);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(GameManager.StateType.PauseMenu);
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.LoadScene(0);
    }
}
