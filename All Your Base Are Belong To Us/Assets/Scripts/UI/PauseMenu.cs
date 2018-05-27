using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    //public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

	// Update is called once per frame
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
            if (GameManager.Instance.GetGameState() == GameManager.StateType.PauseMenu)
                Resume();

            if (GameManager.Instance.GetGameState() == GameManager.StateType.Options)
            {
                GetComponentInChildren<OptionsMenu>().GoBack();
                pauseMenuUI.SetActive(true);
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
