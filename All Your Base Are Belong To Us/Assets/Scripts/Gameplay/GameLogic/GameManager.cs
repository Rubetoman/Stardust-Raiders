using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

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
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [System.Serializable]
    public class PlayerInfo
    {
        public const int startingLives = 3;
        public int lives = startingLives;
        public Text livesCount;
        public bool isDead = false;
    }
    public enum StateType
    {
        PLAY,
        MENU,         // Player is viewing in-game menu
        OPTIONS,      // Player is adjusting game options
        PAUSE,
        GAMEOVER
    };
    public Object[] levelScenes;
    public Object introScene;
    public Object mainMenuScene;
    public PlayerInfo playerInfo;

    //private GameObject player;
    private int TotalScore { get; set; }
    private int ActualScene { get; set; }
    private bool PlayerDead { get; set; }

    private void OnGUI()
    {
        GUILayout.Label("Score: " + TotalScore);
    }
    // Use this for initialization
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
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
        TotalScore = 0;
    }
    #endregion

    #region PlayerFunctions
    /// <summary>
    /// Sets player dead or alive
    /// </summary>
    /// <param name="state">true to set it dead, false otherwise</param>
    public void SetPlayerDead(bool state)
    {
        if(playerInfo.isDead != state)
            playerInfo.isDead = state;
    }

    /// <summary>
    /// Set a custom amount of lives for the player
    /// </summary>
    /// <param name="number">number of lives to set</param>
    public void SetPlayerLives(int number)
    {
        playerInfo.lives = number;
        playerInfo.livesCount.text = "x" + playerInfo.lives;
    }

    /// <summary>
    /// Substracts the given number of lives, if it reaches 0 (minimun value) sets player dead
    /// </summary>
    /// <param name="amount">number of lives to substract</param>
    public void SubstractPlayerLives(int amount)
    {
        if (playerInfo.lives - amount < 0)
        {
            playerInfo.lives = 0;
            SetPlayerDead(true);
        }else
            playerInfo.lives -= amount;

        playerInfo.livesCount.text = "x" + playerInfo.lives;
    }

    /// <summary>
    /// Adds to the live count of the player the given amount, if the player was dead it is revived
    /// </summary>
    /// <param name="amount">number of lives to add</param>
    public void AddtPlayerLives(int amount)
    {
        if (playerInfo.isDead)
            SetPlayerDead(false);
        playerInfo.lives += amount;
        playerInfo.livesCount.text = "x" + playerInfo.lives;
    }
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

    public void SetScene(string sceneName)
    {
        ActualScene = SceneManager.GetSceneByName(sceneName).buildIndex;
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
