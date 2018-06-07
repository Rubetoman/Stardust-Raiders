using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {
    #region SingletonAndAwake
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null)
            {
                Resources.Load("Prefab/GameManagement/GameManager");
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [System.Serializable]
    public class PlayerInfo
    {
        public const int startingLives = 3;
        public int lives = startingLives;
        public bool isDead = false;
        public GunType gunType;             // Type of shoot the player will use
        public bool invertXAxis = false;    // Invert horizontal movement (true for invert)
        public bool invertYAxis = true;    // Invert vertical movement (true for invert) 
    }
    public enum StateType
    {
        Play,           // Player is playing a game level
        MainMenu,       // Player is viewing main menu
        Options,        // Player is adjusting game options
        PauseMenu,      // Player is viewing in-game menu
        Gameover,       // Player is dead and out of lifes
        Credits         // Player has already win the game or is viewing the game credits
    };

    public StateType gameState;
    public GameObject loadingScreen;
    public Slider loadSlider;
    public Text progressText;
    public PlayerInfo playerInfo;

    private int TotalScore { get; set; }
    private bool PlayerDead { get; set; }

    // Use this for initialization
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        TotalScore = 0;
    }


    /// <summary>
    /// Resets GameManager by seting the TotalScore to 0, Player values to default and reloads current scene
    /// </summary>
    public void ResetGameManager()
    {
        ResetPlayerInfo();
        ResetTotalScore();
        //ResetScene();
        if(PlayerHUDManager.Instance!=null)
            PlayerHUDManager.Instance.ResetHUD();
    }

    #region Game State
    public void SetGameState(StateType state)
    {
        if (gameState != state)
            gameState = state;
        else
            Debug.LogWarning("State already in " + state.ToString());
    }

    public StateType GetGameState()
    {
        return gameState;
    }
    #endregion

    #region ScoreFunctions
    /// <summary>
    /// Returns the Total Score
    /// </summary>
    /// <returns>Int of the total score</returns>
    public int GetTotalScore()
    {
        return TotalScore;
    }

    /// <summary>
    /// Adds the amount passed as parameter to the Total Score
    /// </summary>
    /// <param name="amount">Amount of points to add</param>
    public void AddToTotalScore(int amount)
    {
        TotalScore += amount;
    }

    /// <summary>
    /// Substracts the amount passed as parameter from Total Score, it can go below 0.
    /// </summary>
    /// <param name="amount">Amount to substract</param>
    public void SubstractToTotalScore(int amount)
    {
        if (TotalScore - amount >= 0)
            TotalScore -= amount;
        else
            Debug.LogError("TotalScore can't go below 0");
    }

    /// <summary>
    /// Sets the Total Score back to 0
    /// </summary>
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
        if(PlayerHUDManager.Instance != null)
            PlayerHUDManager.Instance.SetDisplayedLifes(playerInfo.lives);
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
        if (PlayerHUDManager.Instance != null)
            PlayerHUDManager.Instance.SetDisplayedLifes(playerInfo.lives);
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
        if (PlayerHUDManager.Instance != null)
            PlayerHUDManager.Instance.SetDisplayedLifes(playerInfo.lives);
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
        SetPlayerLives(PlayerInfo.startingLives);
        playerInfo.isDead = false;
    }
    #endregion

    #region SceneFunctions
    /// <summary>
    /// Loads next scene by buildIndex
    /// </summary>
    public void NextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Loads previous scene by buildIndex
    /// </summary>
    public void PreviousScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /// <summary>
    /// Loads a scene by his number on the Build Index
    /// </summary>
    /// <param name="sceneNumber">Number of the scene on the Build Index</param>
    public void LoadScene(int sceneNumber)
    {
        if(sceneNumber < 0)
            Debug.LogError("Can't load a scene with a number lower than 0. Scene numbers on the Build Index start at 0.");
        else if (sceneNumber >= SceneManager.sceneCountInBuildSettings)
            Debug.LogError("There isn't any scene with a number as high as that. Higher scene number is: " + (SceneManager.sceneCountInBuildSettings-1));
        else
        {
            if(sceneNumber == 0)    // main_menu
            {
                gameState = StateType.MainMenu;
                ResetGameManager();
                if (PlayerHUDManager.Instance != null)
                    PlayerHUDManager.Instance.HidePlayerHUD(true);
            }
            else if(sceneNumber == SceneManager.sceneCount - 1)   //credits
            {
                gameState = StateType.Credits;
                ResetGameManager();
                if (PlayerHUDManager.Instance != null)
                    PlayerHUDManager.Instance.HidePlayerHUD(true);
            }
            else
            {
                gameState = StateType.Play;
                if(PlayerHUDManager.Instance!=null)
                    PlayerHUDManager.Instance.HidePlayerHUD(false);
            }
            StartCoroutine(LoadAsync(sceneNumber));
        }
    }

    /// <summary>
    /// Loads a scene by his name
    /// </summary>
    /// <param name="sceneNumber">Name of the scene</param>
    public void LoadScene(string sceneName)
    {
        if (sceneName == "main_menu")    // main_menu
        {
            ResetGameManager();
            if (PlayerHUDManager.Instance != null)
                PlayerHUDManager.Instance.HidePlayerHUD(true);
        }
        else if (sceneName == "credits")    //credits
        {
            gameState = StateType.Credits;
            ResetGameManager();
            if (PlayerHUDManager.Instance != null)
                PlayerHUDManager.Instance.HidePlayerHUD(true);
        }
        else
        {
            if (PlayerHUDManager.Instance != null)
                PlayerHUDManager.Instance.HidePlayerHUD(false);
        }
        StartCoroutine(LoadAsync(sceneName));
    }

    /// <summary>
    /// Loads the current scene
    /// </summary>
    public void ResetScene()
    {
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
    }
    
    /// <summary>
    /// This Couroutine will load the scene by index while showing a load screen.
    /// </summary>
    /// <param name="sceneIndex"> Number of the scene on the build scenes</param>
    IEnumerator LoadAsync(int sceneIndex)
    {
        Time.timeScale = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadSlider.value = progress;
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }
        loadingScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// This Couroutine will load the scene by index while showing a load screen.
    /// </summary>
    /// <param name="sceneName"> Name of the scene to load</param>
    IEnumerator LoadAsync(string sceneName)
    {
        Time.timeScale = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        if (operation == null)
            yield break;
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadSlider.value = progress;
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }
        loadingScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    #endregion

}
