using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region SingletonAndAwake
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (Instance == null)
            DontDestroyOnLoad(gameObject);
        else if (Instance != this)
            Destroy(gameObject);

    }
    #endregion

    public enum StateType
    {
        PLAY,
        MENU,         // Player is viewing in-game menu
        OPTIONS,      // Player is adjusting game options
        PAUSE,      
        GAMEOVER
    };
    //public Object[] levelScenes;
    //public Object introScene;
    //public Object mainMenuScene;
    //public GameObject player;

    private int TotalScore { get; set; }
    private int ActualScene { get; set; }
    private bool PlayerDead { get; set; }


    // Use this for initialization
    void Start()
    {
        TotalScore = 0;
        ActualScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update () {
		
	}

    #region ScoreFunctions
    public void AddToTotalScore(int amount)
    {
        TotalScore += amount;
    }

    public void SubstractToTotalScore(int amount)
    {
        if (TotalScore - amount >= 0)
            TotalScore -= amount;
        else
            Debug.LogError("TotalScore can't go below 0");
    }

    public void ResetTotalScore()
    {
        
    }
    #endregion

    #region LevelFunctions
    /*public void NextLevel()
    {
        TotalScore++;
    }

    public void PreviousLevel()
    {

    }

    public void SetLevel(int levelNumber)
    {
       
    }*/
    #endregion

    #region SceneFunctions
    public void NextScene()
    {
        ActualScene++;
        SceneManager.LoadScene(ActualScene);
    }

    public void PreviousScene()
    {
        ActualScene--;
        SceneManager.LoadScene(ActualScene);
    }

    public void SetScene(int sceneNumber)
    {
        ActualScene = sceneNumber;
        SceneManager.LoadScene(sceneNumber);
    }
    #endregion
}
