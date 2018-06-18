using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OptionsMenu : MonoBehaviour {
    public AudioMixer audioMixer;
    public TMP_Dropdown graphicQualityDropdown;
    public Dropdown resolutionDropdown;
    public Toggle fullscreen;
    public Toggle invertXAxis;
    public Toggle invertYAxis;

    private Resolution[] resolutions;
    private GameManager.StateType previousState;
    private int currentResolutionIndex;
    private GameObject myEventSystem;
    private GameObject lastSelected;
    void Awake()
    {
        AddCurrentGameResolutionsToDropdown();          // Add all resolution options to dropdown.
        ShowCurrentOptionsValues();                     // Show current game options values.
        myEventSystem = GameObject.Find("EventSystem"); // Set scene EventSystem.
    }

    private void OnEnable()
    {
        previousState = GameManager.Instance.GetGameState();                // Save previous state (could be MainMenu or PauseMenu).
        GameManager.Instance.SetGameState(GameManager.StateType.Options);   // Set current state to Options.

        lastSelected = graphicQualityDropdown.gameObject;                   // Use the first button of the options menu for the selection EventSystem.
        if(myEventSystem != null)
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(lastSelected);
    }

    private void OnDisable()
    {
        // Once disabled use the first button of the previous menu as selected.
        if (myEventSystem != null)
            myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(myEventSystem.GetComponent<EventSystem>().firstSelectedGameObject);
    }

    private void Update()
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
    /// <summary>
    /// Adds the available resolutions to the Resolution Dropdown. This resolutions can vary between executing systems.
    /// </summary>
    private void AddCurrentGameResolutionsToDropdown()
    {
        resolutions = Screen.resolutions;   // Create an array with all the resolutions
        resolutionDropdown.ClearOptions();  // Clear all options form dropdown

        List<string> options = new List<string>();
        currentResolutionIndex = 0;
        // Pass every resolution on the array to string
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options); // Add resolutions to the Dropdown
    }

    /// <summary>
    /// Shows, on the corresponding UI elements, the current game values.
    /// </summary>
    private void ShowCurrentOptionsValues()
    {
        resolutionDropdown.value = currentResolutionIndex;                  // Show current resolution by default
        resolutionDropdown.RefreshShownValue();
        graphicQualityDropdown.value = QualitySettings.GetQualityLevel();   // Show current graphic quality by default
        graphicQualityDropdown.RefreshShownValue();
        // Only look for fullscreen when running an actual build of the game
        #if UNITY_STANDALONE && !UNITY_EDITOR
        fullscreen.isOn = Screen.fullScreen;
        #endif
        invertXAxis.isOn = GameManager.Instance.playerInfo.invertXAxis;
        invertYAxis.isOn = GameManager.Instance.playerInfo.invertYAxis;
    }

    /// <summary>
    /// Sets the game resolution with the value passed as parameter.
    /// </summary>
    /// <param name="resolutionIndex"> Index number of the resolution list.</param>
    public void SetResolution ( int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// Sets the game music volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetMusicVolume (float volume)
    {
        AudioManager.Instance.SetGameMusicVolume(volume);
    }

    /// <summary>
    /// Sets the game sound effects volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetSoundEffectsVolume(float volume)
    {
        AudioManager.Instance.SetGameSoundEffectsVolume(volume);
    }

    /// <summary>
    /// Sets the game graphic quality.
    /// </summary>
    /// <param name="qualityIndex"> Number of the index on the project settings.</param>
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Toggles the game between Fullscreen or Window mode.
    /// </summary>
    /// <param name="isFullscreen"> Using Full Screen or not</param>
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (!isFullscreen)
        {
            Resolution resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        }
    }

    /// <summary>
    /// Set input that controls X Axis player movement inverted or not.
    /// </summary>
    /// <param name="invert"> Invert the X Axis or not.</param>
    public void SetXAxis(bool invert)
    {
        GameManager.Instance.playerInfo.invertXAxis = invert;
    }

    /// <summary>
    /// Set input that controls Y Axis player movement inverted or not.
    /// </summary>
    /// <param name="invert"> Invert the Y Axis or not.</param>
    public void SetYAxis(bool invert)
    {
        GameManager.Instance.playerInfo.invertYAxis = invert;
    }

    /// <summary>
    /// Function to go back to the previous menu.
    /// </summary>
    public void GoBack()
    {
        GameManager.Instance.SetGameState(previousState);
        gameObject.SetActive(false);
    }

    public void PlaySoundClip(string name)
    {
        AudioManager.Instance.Play(name);
    }
}
