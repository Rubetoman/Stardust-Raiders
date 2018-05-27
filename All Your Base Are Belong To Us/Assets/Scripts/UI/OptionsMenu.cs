using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private GameManager.StateType previousState;
    void Start()
    {
        

        resolutions = Screen.resolutions;   // Create an array with all the resolutions
        resolutionDropdown.ClearOptions();  // Clear all options form dropdown

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        // Pass every resolution on the array to string
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);             // Add resolutions to the Dropdown
        resolutionDropdown.value = currentResolutionIndex;  // Show current resolution by default
        resolutionDropdown.RefreshShownValue();
    }
    private void OnEnable()
    {
        previousState = GameManager.Instance.GetGameState();
        GameManager.Instance.SetGameState(GameManager.StateType.Options);
    }
    private void Update()
    {
        /*if (Input.GetButtonDown("Cancel"))
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.Options)
                GoBack();
        }*/
    }

    public void SetResolution ( int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
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

    public void GoBack()
    {
        GameManager.Instance.SetGameState(previousState);
        gameObject.SetActive(false);
    }
}
