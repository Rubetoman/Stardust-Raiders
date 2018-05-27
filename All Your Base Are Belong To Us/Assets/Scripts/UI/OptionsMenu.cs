using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
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

    void Start()
    {
        AddCurrentGameResolutionsToDropdown();  // Add all resolution options to dropdown
        ShowCurrentOptionsValues();             // Show current game options values
    }

    private void OnEnable()
    {
        previousState = GameManager.Instance.GetGameState();
        GameManager.Instance.SetGameState(GameManager.StateType.Options);
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
    /// Sets the game volume value to the one given as parameter.
    /// </summary>
    /// <param name="volume"> Volume value.</param>
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
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
}
