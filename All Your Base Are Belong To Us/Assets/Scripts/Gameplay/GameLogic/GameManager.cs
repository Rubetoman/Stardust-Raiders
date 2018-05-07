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
        public GunType gunType;             // Type of shoot the player will use
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

    /// <summary>
    /// Resets GameManager by seting the TotalScore to 0, Player values to default and reloads current scene
    /// </summary>
    public void ResetGameManager()
    {
        ResetPlayerInfo();
        ResetTotalScore();
        ResetScene();
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
    /// Substracts the given number of lives, if it reaches 0 (minimun value) sets player dead. Also resets player GunType
    /// </summary>
    /// <param name="amount">number of lives to substract</param>
    public void SubstractPlayerLives(int amount)
    {
        if (playerInfo.lives - amount < 0)
        {
            playerInfo.lives = 0;
            SetPlayerDead(true);
        }
        else
        {
            playerInfo.lives -= amount;
            ResetGunType();
        }
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

    /// <summary>
    /// Upgrades player gun: single->dual->triple
    /// </summary>
    public void UpgradeGunType()
    {
        if (playerInfo.gunType == GunType.Single)
            playerInfo.gunType = GunType.Dual;
        else if (playerInfo.gunType == GunType.Dual)
            playerInfo.gunType = GunType.Triple;
    }

    /// <summary>
    /// Sets player gun back at the first type of gun
    /// </summary>
    public void ResetGunType()
    {
        if (playerInfo.gunType != GunType.Single)
            playerInfo.gunType = GunType.Single;
    }

    /// <summary>
    /// Sets the gun type choosen.
    /// </summary>
    /// <param name="gun">Type of gun</param>
    public void SetGunType(GunType gun)
    {
        if (playerInfo.gunType != gun)
            playerInfo.gunType = gun;
    }

    /// <summary>
    /// Resets PlayerInfo variables to the default value.
    /// </summary>
    void ResetPlayerInfo()
    {
        ResetGunType();
        playerInfo.lives = PlayerInfo.startingLives;
        playerInfo.isDead = false;
    }
    #endregion

    #region SceneFunctions
    /// <summary>
    /// Loads next scene by buildIndex
    /// </summary>
    public void NextScene()
    {
        if(ActualScene < SceneManager.sceneCountInBuildSettings)
        {
            ActualScene++;
            SceneManager.LoadScene(ActualScene);
        }
        else
            Debug.LogError("This is the last scene on the Build Index, can't load next scene.");
    }

    /// <summary>
    /// Loads previous scene by buildIndex
    /// </summary>
    public void PreviousScene()
    {
        if (ActualScene > 0)
        {
            ActualScene--;
            SceneManager.LoadScene(ActualScene);
        }
        else
            Debug.LogError("This is the first scene on the Build Index, can't load a previous scene.");
    }

    /// <summary>
    /// Loads a scene by his number on the Build Index
    /// </summary>
    /// <param name="sceneNumber">Number of the scene on the Build Index</param>
    public void SetScene(int sceneNumber)
    {
        if(sceneNumber < 0)
            Debug.LogError("Can't load a scene with a number lower than 0. Scene numbers on the Build Index start at 0.");
        else if (sceneNumber >= SceneManager.sceneCountInBuildSettings)
            Debug.LogError("There isn't any scene with a number as high as that. Higher scene number is: " + (SceneManager.sceneCountInBuildSettings-1));
        else
        {
            ActualScene = sceneNumber;
            SceneManager.LoadScene(sceneNumber);
        }
    }

    /// <summary>
    /// Loads a scene by his name
    /// </summary>
    /// <param name="sceneNumber">Name of the scene</param>
    public void SetScene(string sceneName)
    {
        ActualScene = SceneManager.GetSceneByName(sceneName).buildIndex;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Loads the current scene
    /// </summary>
    public void ResetScene()
    {
        SceneManager.LoadScene(ActualScene);
    }
    #endregion
}
