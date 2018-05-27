using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    //public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
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
