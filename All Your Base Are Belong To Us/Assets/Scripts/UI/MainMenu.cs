using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject titleScreenUI;
    public GameObject mainMenuUI;
    public AudioClip loopClip;

    private  GameObject myEventSystem;
    private GameObject lastSelected;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myEventSystem = GameObject.Find("EventSystem");
        if (myEventSystem != null)
            lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;
        StartCoroutine(AudioManager.Instance.ChangeAuioSourceClip(audioSource, loopClip, true, true));
    }
    void Update()
    {
        if (Input.GetButtonDown("Start") && titleScreenUI.activeSelf)
        {
            AudioManager.Instance.Play("Start");
            titleScreenUI.SetActive(false);
            mainMenuUI.SetActive(true);
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);// Start with nothing selected
        }
        if (Input.GetButtonDown("Back"))
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.Options)
            {
                GetComponentInChildren<OptionsMenu>().GoBack();
                mainMenuUI.SetActive(true);
                lastSelected = myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject;
                myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
            }
            else if(GameManager.Instance.GetGameState() == GameManager.StateType.MainMenu && mainMenuUI.activeSelf)
            {
                mainMenuUI.SetActive(false);
                titleScreenUI.SetActive(true);
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
        GetComponent<AudioSource>().Stop();
        GameManager.Instance.SetGameState(GameManager.StateType.Play);
        GameManager.Instance.NextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region SoundEffects
    public void PlaySoundClip(string name)
    {
        AudioManager.Instance.Play(name);
    }
    #endregion
}
