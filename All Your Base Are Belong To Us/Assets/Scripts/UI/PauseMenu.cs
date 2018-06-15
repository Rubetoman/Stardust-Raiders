using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenuUI;
    public GameObject mainMenuChooser;
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

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
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
                break;
            case GameManager.StateType.Play:
                Pause();
                break;
            }
        }
        if (GameManager.Instance.GetGameState() == GameManager.StateType.PauseMenu && !mainMenuChooser.activeInHierarchy)
        {
            // If no UI gameObject was selected and horizontal or vertical Input is detected, select the last selected button.
            if (myEventSystem.GetComponent<EventSystem>().currentSelectedGameObject == null)
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
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
        unpaused.TransitionTo(0);
        Time.timeScale = 1f;
        GameManager.Instance.SetGameState(GameManager.StateType.Play);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        paused.TransitionTo(0);
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);// Start with nothing selected
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(GameManager.StateType.PauseMenu);
    }

    public void LoadMainMenu()
    {
        unpaused.TransitionTo(0);
        GameManager.Instance.LoadScene(0);
    }

    public void PlaySoundClip(string name)
    {
        AudioManager.Instance.Play(name);
    }
}
