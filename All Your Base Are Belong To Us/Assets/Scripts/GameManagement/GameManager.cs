using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    #region Singleton
    // Declare an instance of GameManager.
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            // If the scene doesn't contain an instance load the prefab.
            if (_instance == null) { Resources.Load("Prefab/GameManagement/GameManager"); }               
            return _instance;
        }
    }
    #endregion

    /// <summary>
    /// Class that contains information about the current Player.
    /// It has been declared serializable in case multiplayer is added in the future.
    /// </summary>
    [System.Serializable]
    public class PlayerInfo
    {
        public const int startingLives = 3; // Number of lives that the Player is given when spawning.
        public int lives = startingLives;   // Current number of lives the player has.
        public bool isDead = false;         // If the Player is dead or not.
        public GunType gunType;             // Type of gun the player currently has.
        public bool invertXAxis = false;    // Invert horizontal movement (true for invert).
        public bool invertYAxis = true;     // Invert vertical movement (true for invert).
    }

    /// <summary>
    /// Enumum to define the state of the game.
    /// </summary>
    public enum StateType
    {
        Play,           // Player is playing a game level.
        MainMenu,       // Player is on the main menu.
        Options,        // Player is adjusting game options.
        PauseMenu,      // Player is viewing in-game menu.
        Gameover,       // Player is dead and out of lifes.
        Credits         // Player has already win the game or is viewing the game credits.
    };
    public GameObject player;           // Player GameObject (updated on each scene).
    public StateType gameState;         // State of the game.
    public GameObject loadingScreen;    // Screen that will show up when game is loading a new scene.
    public Slider loadSlider;           // Slider that shows the progress of load for the new scene.
    public Text progressText;           // Text of the loading screen.
    public PlayerInfo playerInfo;       // Information of the Player.

    private int TotalScore { get; set; }    // Score of the game. 
    private bool loading = false;           // Bool that tells if a new scene is being loaded. (Used to avoid loading a new scene while already loading one).

    void Awake()
    {
        #region instace Code
        if (_instance == null) { _instance = this; }            // If the instance is not set, set this GameManager as it.
        else if (_instance != this) { Destroy(gameObject); }    // If another instance already exist auto destroy.
        DontDestroyOnLoad(gameObject);                          // Avoid destroying the instance when changing scene.
        #endregion

        SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameObject.FindGameObjectWithTag("Player");    // Set player variable.
        TotalScore = 0;                                         // Start with score at 0.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");    // Update Player GameObject on scene loaded.
    }

    /// <summary>
    /// Resets GameManager by seting the TotalScore to 0, Player values to default and resets Player HUD if is on the scene.
    /// </summary>
    public void ResetGameManager()
    {
        ResetPlayerInfo();
        ResetTotalScore();
        if(PlayerHUDManager.Instance!=null)
            PlayerHUDManager.Instance.ResetHUD();
    }

    /// <summary>
    /// Resets current level by reseting the GameManager, reloading the level and seting the state back to Play.
    /// </summary>
    public void ResetLevel()
    {
        SetGameState(StateType.Play);
        ResetGameManager();
        ResetScene();
    }

    #region Game State Functions

    /// <summary>
    /// Function to set the game state to the one passed as parameter.
    /// </summary>
    /// <param name="state"> State to change to.</param>
    public void SetGameState(StateType state)
    {
        if (gameState != state)
            gameState = state;
        else
            Debug.LogWarning("State already in " + state.ToString());
    }

    /// <summary>
    /// Function to get current game state.
    /// </summary>
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
    /// Sets player dead or alive.
    /// </summary>
    /// <param name="state"> True to set it dead, false otherwise.</param>
    public void SetPlayerDead(bool state)
    {
        if(playerInfo.isDead != state)
            playerInfo.isDead = state;
    }

    /// <summary>
    /// Set a custom amount of lives for the player.
    /// </summary>
    /// <param name="number"> Number of lives to set</param>
    public void SetPlayerLives(int number)
    {
        playerInfo.lives = number;
        // Update Player HUD.
        if(PlayerHUDManager.Instance != null)
            PlayerHUDManager.Instance.SetDisplayedLifes(playerInfo.lives);
    }

    /// <summary>
    /// Substracts the given number of lives, if it reaches 0 (minimun value) sets player dead. Also resets Player's GunType.
    /// </summary>
    /// <param name="amount"> Number of lives to substract.</param>
    public void SubstractPlayerLives(int amount)
    {
        if (amount > 0) // Avoid negative numbers or 0.
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
            // Update Player HUD.
            if (PlayerHUDManager.Instance != null)
                PlayerHUDManager.Instance.SetDisplayedLifes(playerInfo.lives);
        }
        else if(amount == 0)
        {
                Debug.LogWarning("Attempt of substracting 0 lives has been ignored, it might be an error");
        }
        else
        {
            Debug.LogError("Amount of lives to substract must be a positive number. If you wanted to add lives use AddPlayerLives() instead");
        }
    }

    /// <summary>
    /// Adds to the live count of the player the given amount, if the player was dead it is set alive again.
    /// </summary>
    /// <param name="amount"> Number of lives to add.</param>
    public void AddPlayerLives(int amount)
    {
        if (amount > 0) // Avoid negative numbers or 0.
        {
            if (playerInfo.isDead)
                SetPlayerDead(false);
            playerInfo.lives += amount;
            // Update Player HUD.
            if (PlayerHUDManager.Instance != null)
                PlayerHUDManager.Instance.SetDisplayedLifes(playerInfo.lives);
        }
        else if (amount == 0)
        {
            Debug.LogWarning("Attempt of adding 0 lives has been ignored, it might be an error");
        }
        else
        {
            Debug.LogError("Amount of lives to add must be a positive number. If you wanted to add lives use SubstractPlayerLives() instead");
        }
    }

    /// <summary>
    /// Upgrades player gun: single->dual->triple.
    /// </summary>
    public void UpgradeGunType()
    {
        if (playerInfo.gunType == GunType.Single)
            playerInfo.gunType = GunType.Dual;
        else if (playerInfo.gunType == GunType.Dual)
            playerInfo.gunType = GunType.Triple;
        else
            Debug.LogWarning("Gun already upgraded to highest type.");
    }

    /// <summary>
    /// Sets player gun back at the first type of gun.
    /// </summary>
    public void ResetGunType()
    {
        if (playerInfo.gunType != GunType.Single)
            playerInfo.gunType = GunType.Single;
        else
            Debug.LogWarning("Gun already on lowest type.");
    }

    /// <summary>
    /// Sets the gun type choosen.
    /// </summary>
    /// <param name="gun">Type of gun. </param>
    public void SetGunType(GunType gun)
    {
        if (playerInfo.gunType != gun)
            playerInfo.gunType = gun;
        else
            Debug.LogWarning("Gun already set as " + gun + ".");
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
    /// Loads next scene by buildIndex.
    /// </summary>
    public void NextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Loads previous scene by buildIndex.
    /// </summary>
    public void PreviousScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /// <summary>
    /// Loads a scene by his number on the Build Index.
    /// </summary>
    /// <param name="sceneNumber"> Number of the scene on the Build Index. </param>
    public void LoadScene(int sceneNumber)
    {
        if (loading)    // Exit if a scene is being loaded.
        {
            Debug.LogWarning("Already loading a scene.");
            return;
        }
        if (sceneNumber < 0)
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
            else if(sceneNumber == SceneManager.sceneCountInBuildSettings - 1)   //credits
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
            // Load new scene asynchronously.
            StartCoroutine(LoadAsync(sceneNumber));
        }
    }

    /// <summary>
    /// Loads a scene by his name.
    /// </summary>
    /// <param name="sceneNumber"> Name of the scene.</param>
    public void LoadScene(string sceneName)
    {
        if (loading)    // Exit if a scene is being loaded.
        {
            Debug.LogWarning("Already loading a scene.");
            return;
        }
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
        // Load new scene asynchronously.
        StartCoroutine(LoadAsync(sceneName));
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void ResetScene()
    {
        // Reload it asynchronously.
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
    }
    
    /// <summary>
    /// This Couroutine will load the scene by index while showing a load screen.
    /// </summary>
    /// <param name="sceneIndex"> Number of the scene on the build scenes.</param>
    IEnumerator LoadAsync(int sceneIndex)
    {
        loading = true;                                                     
        Time.timeScale = 0f;                                                // Pause game time.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex); // Load new scene asynchronously.
        if (operation == null)
            yield break;

        loadingScreen.SetActive(true);                                      // Show loading screen.
        while (!operation.isDone)                                           // Show load progress.
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadSlider.value = progress;                                        
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            yield return null;
        }
        loadingScreen.SetActive(false);                                     // Hide loading screen.
        Time.timeScale = 1f;                                                // Resume game time.
        loading = false;
    }

    /// <summary>
    /// This Couroutine will load the scene by index while showing a load screen.
    /// </summary>
    /// <param name="sceneName"> Name of the scene to load</param>
    IEnumerator LoadAsync(string sceneName)
    {
        loading = true;
        Time.timeScale = 0f;                                                // Pause game time.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);  // Load new scene asynchronously.
        if (operation == null)
            yield break;

        loadingScreen.SetActive(true);                                      // Show loading screen.
        while (!operation.isDone)                                           // Show load progress.
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadSlider.value = progress;
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            yield return null;
        }
        loadingScreen.SetActive(false);                                     // Hide loading screen.
        Time.timeScale = 1f;                                                // Resume game time.
        loading = false;
    }

    #endregion
}
