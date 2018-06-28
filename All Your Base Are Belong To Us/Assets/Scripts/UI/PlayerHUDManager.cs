using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Script to manage the Player's HUD. It contains the element shown on the HUD and functions to manage those elements.
/// It is instanciated and not destroyed when changing between scenes.
/// </summary>
public class PlayerHUDManager : MonoBehaviour {

    #region SingletonAndAwake
    private static PlayerHUDManager _instance;
    public static PlayerHUDManager Instance {
        get {
            if (_instance == null)
            {
                Resources.Load("Prefab/HUD/PlayerHUD");
                /*
                GameObject go = new GameObject("PlayerHUDManager");
                go.AddComponent<PlayerHUDManager>();*/
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

    [Header("Life Canvas")]
    public Text livesCount;                             // Text that shows the number of lives of the Player.
    [Space(10)]
    [Header("Shield Bar Canvas")]
    public RectTransform playerShieldBarForeground;     // The rect of the Image of the Shield Bar (The updating bar line, not the border).
    [Space(10)]
    [Header("Boost Bar Canvas")]
    public RectTransform boostBarForeground;            // The rect of the Image of the Boost Bar (The updating bar line, not the border).
    [Space(10)]
    [Header("Limit Canvas")]
    public GameObject[] limitArrows;                    // The UI arrows which appear when you reach the limit. Must be inserted in the following order: up, low, left, right.
    [Space(10)]
    [Header("Path Selection Canvas")]
    public GameObject[] selectionArrows;                // The UI arrows which appear pointing both paths. Must be inserted in the following order: up, low, left, right.
    public GameObject PathSelectionText;                // The UI Text that shows when a path selection starts.
    [Space(10)]
    [Header("Enemy Shield Canvas")]
    public GameObject enemyShieldBar;                   // The GameObject of the whole Enemy Shield Bar.
    public RectTransform enemyShieldBarForeground;      // The rect of the Image of the Enemy Shield Bar (The updating bar line, not the border).
    [Space(10)]
    [Header("Game Over Canvas")]
    public Image gameOverPanel;                         // Panel where all elements of the GameOver Screen are located.
    public Text score;                                  // Text tha will show the Total Score on the Game Over Screen.
    public AudioMixerSnapshot gameOver;                 // Audio Mixer Snapshot to use when the game is on the Game Over screen. 
    public AudioMixerSnapshot gamePlay;                 // Audio Mixer default Snapshot.
    [Header("Hit Canvas")]
    public Image hitScreen;                             // Image that will flash when the Player is hurt.

    private void Start()
    {
        // Check that the all the limit arrows are assigned.
        if (limitArrows.Length < 4)
            Debug.LogWarning("There are arrows missing. Make sure to insert the 4 arrows in the following order: upper arrow, lower arrow, left arrow, right arrow");

        // Check that the all the selection arrows are assigned.
        if (selectionArrows.Length < 4)
            Debug.LogWarning("There are arrows missing. Make sure to insert the 4 arrows in the following order: upper arrow, lower arrow, left arrow, right arrow");

        // Get the Player lives number to show them.
        if(GameManager.Instance != null)
            SetDisplayedLifes(GameManager.Instance.playerInfo.lives);
    }

    /// <summary>
    /// Hides PlayerHUD by SetingInactive each child canvas.
    /// </summary>
    /// <param name="hide"> If hide the HUD or not.</param>
    public void HidePlayerHUD(bool hide)
    {
        foreach(RectTransform child in gameObject.GetComponentsInChildren<RectTransform>(true))
        {
            // Only SetActive childs of one level of depth.
            if (child.gameObject.activeSelf == hide && child.parent.name == gameObject.name) 
            {
                child.gameObject.SetActive(!hide);
            }
        }
    }

    /// <summary>
    /// Funtion to reset all HUD elements.
    /// </summary>
    public void ResetHUD()
    {
        ResetPlayerShieldBar();
        ResetBoostBar();
        ResetLimitArrows();
        ResetPathSelection();
        ResetEnemyShieldBar();
        ResetGameOverScreen();
    }

    /// <summary>
    /// Funtion to set the Text of the lives count.
    /// </summary>
    /// <param name="lifes"></param>
    public void SetDisplayedLifes(int lifes)
    {
        livesCount.text = "x" + lifes;
    }

    #region Shield Bar Canvas

    /// <summary>
    /// Resets Player's Shield Bar on the HUD.
    /// </summary>
    public void ResetPlayerShieldBar()
    {
        playerShieldBarForeground.sizeDelta = new Vector2(100, 7.5f);
    }

    /// <summary>
    /// Sets Player's Shield Bar Width on the HUD.
    /// </summary>
    public void SetPlayerShieldBarWidth(float width)
    {
        playerShieldBarForeground.sizeDelta = new Vector2(width, playerShieldBarForeground.sizeDelta.y);
    }

    /// <summary>
    /// Sets Player's Shield Bar Height on the HUD.
    /// </summary>
    public void SetPlayerShieldBarHeight(float height)
    {
        playerShieldBarForeground.sizeDelta = new Vector2(playerShieldBarForeground.sizeDelta.x, height);
    }

    /// <summary>
    /// Gets Player's Shield Bar Width from the HUD.
    /// </summary>
    public int GetPlayerShieldBarWidth()
    {
        return Mathf.RoundToInt(playerShieldBarForeground.sizeDelta.x);
    }

    #endregion

    #region Boost Bar Canvas

    /// <summary>
    /// Resets Player's Boost Bar on the HUD.
    /// </summary>
    public void ResetBoostBar()
    {
        boostBarForeground.sizeDelta = new Vector2(100, 7.5f);
    }

    /// <summary>
    /// Sets Player's Boost Bar Width on the HUD.
    /// </summary>
    public void SetBoostBarWidth(float width)
    {
        boostBarForeground.sizeDelta = new Vector2(width, boostBarForeground.sizeDelta.y);
    }

    /// <summary>
    /// Sets Player's Boost Bar Height on the HUD.
    /// </summary>
    public void SetBoostBarHeight(float height)
    {
        boostBarForeground.sizeDelta = new Vector2(boostBarForeground.sizeDelta.x, height);
    }

    #endregion

    #region Limit Canvas

    /// <summary>
    /// Resets Limit arrows from the Limit Canvas. Sets the arrows innactive.
    /// </summary>
    public void ResetLimitArrows()
    {
        foreach (GameObject arrow in limitArrows)
        {
            if (arrow.activeSelf)
                arrow.SetActive(false);
        }
    }

    /// <summary>
    /// Set the selected arrow from Limit Canvas active or innactive.
    /// </summary>
    public void SetLimitArrowActive(int arrowNumber, bool value)
    {
        if (limitArrows[arrowNumber].activeSelf != value)
            limitArrows[arrowNumber].SetActive(value);
    }

    #endregion

    #region Path Selection Canvas

    /// <summary>
    /// Resets Limit arrows from the Path Selection Canvas. Sets the arrows innactive.
    /// </summary>
    public void ResetPathSelection()
    {
        if (PathSelectionText.activeSelf)
            PathSelectionText.SetActive(false);
        foreach ( GameObject arrow in selectionArrows)
        {
            if(arrow.activeSelf)
            arrow.SetActive(false);
        }
    }

    /// <summary>
    /// Set the selected arrow from Path Selection Canvas active or innactive.
    /// </summary>
    public void SetSelectionArrowActive(int arrowNumber, bool value)
    {
        if(selectionArrows[arrowNumber].activeSelf != value)
            selectionArrows[arrowNumber].SetActive(value);
    }

    /// <summary>
    /// Set the Text from Path Selection Canvas active or innactive.
    /// </summary>
    public void SetPathSelectionTextActive(bool value)
    {
        if(PathSelectionText.activeSelf != value)
            PathSelectionText.SetActive(value);
    }

    #endregion

    #region Enemy Shield Bar Canvas

    /// <summary>
    /// Sets the Enemy Shield Bar Canvas active or innactive.
    /// </summary>
    /// <param name="value"></param>
    public void SetEnemyShieldBarActive(bool value)
    {
        if(enemyShieldBar.activeSelf != value)
            enemyShieldBar.SetActive(value);
    }

    /// <summary>
    /// Resets Enemy Shield Bar on the HUD.
    /// </summary>
    public void ResetEnemyShieldBar()
    {
        enemyShieldBarForeground.sizeDelta = new Vector2(500, 10);
    }

    /// <summary>
    /// Sets Enemy Shield Bar Width on the HUD.
    /// </summary>
    public void SetEnemyShieldBarWidth(float width)
    {
        enemyShieldBarForeground.sizeDelta = new Vector2(width, enemyShieldBarForeground.sizeDelta.y);
    }

    /// <summary>
    /// Sets Enemy Shield Bar Height on the HUD.
    /// </summary>
    public void SetEnemyShieldBarHeight(float height)
    {
        enemyShieldBarForeground.sizeDelta = new Vector2(enemyShieldBarForeground.sizeDelta.x, height);
    }

    #endregion

    #region Game Over Canvas

    /// <summary>
    /// Function that resets elements from Game Over Canvas.
    /// </summary>
    public void ResetGameOverScreen()
    {
        gamePlay.TransitionTo(0);
        score.text = "Score: ";
        var newScreenColor = gameOverPanel.color;
        newScreenColor.a = 0f;
        gameOverPanel.color = newScreenColor;
    }

    /// <summary>
    /// Function to manage the GameOver screen.
    /// </summary>
    public void ShowGameOverScreen()
    {
        gameOver.TransitionTo(0);                                           // Use the Snapshot for Game Over. 
        GameManager.Instance.SetGameState(GameManager.StateType.Gameover);  // Set game state to Gameover
        gameOverPanel.gameObject.SetActive(true);                           // Show Game Over screen.
        StartCoroutine("GameOverAnimation");                                // Start the Game Over animation.
    }

    /// <summary>
    /// Actual animation for the GameOver screen.
    /// </summary>
    private IEnumerator GameOverAnimation()
    {
        float t = 0.0f;
        var screenColor = gameOverPanel.color;
        var newScreenColor = screenColor;
        newScreenColor.a = 1f;
        score.text = "Score: " + GameManager.Instance.GetTotalScore();  // Set score Text with the total score.

        // Smoothly fade to the Game Over screen.
        while (t < 1)
        {
            t += Time.deltaTime;
            gameOverPanel.color = Color.Lerp(screenColor, newScreenColor, t);
            yield return null;
        }
        Time.timeScale = 0f;    // Pause time.
    }

    /// <summary>
    /// Restarts current level.
    /// </summary>
    public void RetryLevel()
    {
        gamePlay.TransitionTo(0);                                       // Use the default Snapshot. 
        GameManager.Instance.SetGameState(GameManager.StateType.Play);  // Set game state to Play.
        GameManager.Instance.ResetGameManager();                        // Reset the GameManager.
        GameManager.Instance.ResetScene();                              // Load the same scene.
    }

    #endregion

    /// <summary>
    /// Animation to be played when the Player is hit. It shows a flashing Image.
    /// </summary>
    public IEnumerator HitEffect()
    {
        hitScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hitScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// Plays a audio clip included on Audio Manager.
    /// </summary>
    /// <param name="name"> Name of the audio clip to play.</param>
    public void PlaySoundClip(string name)
    {
        AudioManager.Instance.Play(name);
    }
}
