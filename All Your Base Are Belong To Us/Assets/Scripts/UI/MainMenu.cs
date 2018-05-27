using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject mainMenuUI;
    void Update()
    {
        if (Input.GetButtonDown("Back"))
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.Options)
            {
                GetComponentInChildren<OptionsMenu>().GoBack();
                mainMenuUI.SetActive(true);
            }
        }

    }
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.SetGameState(GameManager.StateType.Play);
        GameManager.Instance.NextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
