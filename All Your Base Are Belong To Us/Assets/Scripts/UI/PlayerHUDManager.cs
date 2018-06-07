using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    public Text livesCount;
    [Space(10)]
    [Header("Shield Bar Canvas")]
    public RectTransform playerShieldBarForeground;
    [Space(10)]
    [Header("Boost Bar Canvas")]
    public RectTransform boostBarForeground;
    [Space(10)]
    [Header("Limit Canvas")]
    public GameObject[] limitArrows;        // The UI arrows which appear when you reach the limit. Must be inserted in the following order: up, low, left, right.
    [Space(10)]
    [Header("Path Selection Canvas")]
    public GameObject[] selectionArrows;    // The UI arrows which appear pointing both paths. Must be inserted in the following order: (up, low, left, right).
    public GameObject PathSelectionText;
    [Space(10)]
    [Header("Enemy Shield Canvas")]
    public GameObject enemyShieldBar;
    public RectTransform enemyShieldBarForeground;
    [Space(10)]
    [Header("Game Over Canvas")]
    public Image gameOverPanel;         // Panel where all elements of the GameOver Screen are located
    /*public Image gameOverScreen;        // Background Image of the Game Over Screen
    public Text gameOverText;           // Text to be shown on Game Over*/
    public Text score;                  // Text tha will show the Total Score on the Game Over Screen
    public AudioMixerSnapshot gameOver;
    public AudioMixerSnapshot gamePlay;

    private void Start()
    {
        if (limitArrows.Length < 4)
        {
            Debug.LogWarning("There are arrows missing. Make sure to insert the 4 arrows in the following order: upper arrow, lower arrow, left arrow, right arrow");
        }

        if (selectionArrows.Length < 4)
        {
            Debug.LogWarning("There are arrows missing. Make sure to insert the 4 arrows in the following order: upper arrow, lower arrow, left arrow, right arrow");
        }
        if(GameManager.Instance != null)
            SetDisplayedLifes(GameManager.Instance.playerInfo.lives);
    }

    /// <summary>
    /// Hides PlayerHUD by SetingInactive each child canvas.
    /// </summary>
    /// <param name="hide"></param>
    public void HidePlayerHUD(bool hide)
    {
        foreach(RectTransform child in gameObject.GetComponentsInChildren<RectTransform>(true))
        {
            if (child.gameObject.activeSelf == hide && child.parent.name == gameObject.name) // Only SetActive childs of one level of depth
            {
                child.gameObject.SetActive(!hide);
            }

        }
    }

    public void ResetHUD()
    {
        ResetPlayerShieldBar();
        ResetBoostBar();
        ResetLimitArrows();
        ResetPathSelection();
        ResetEnemyShieldBar();
        ResetGameOverScreen();
    }

    public void SetDisplayedLifes(int lifes)
    {
        livesCount.text = "x" + lifes;
    }
    #region Shield Bar Canvas
    public void ResetPlayerShieldBar()
    {
        playerShieldBarForeground.sizeDelta = new Vector2(100, 7.5f);
    }

    public void SetPlayerShieldBarWidth(float width)
    {
        playerShieldBarForeground.sizeDelta = new Vector2(width, playerShieldBarForeground.sizeDelta.y);
    }

    public void SetPlayerShieldBarHeight(float height)
    {
        playerShieldBarForeground.sizeDelta = new Vector2(playerShieldBarForeground.sizeDelta.x, height);
    }
    #endregion

    #region Boost Bar Canvas
    public void ResetBoostBar()
    {
        boostBarForeground.sizeDelta = new Vector2(100, 7.5f);
    }

    public void SetBoostBarWidth(float width)
    {
        boostBarForeground.sizeDelta = new Vector2(width, boostBarForeground.sizeDelta.y);
    }

    public void SetBoostBarHeight(float height)
    {
        boostBarForeground.sizeDelta = new Vector2(boostBarForeground.sizeDelta.x, height);
    }
    #endregion

    #region Limit Canvas
    /// <summary>
    /// Set arrows from Limit canvas innactive
    /// </summary>
    public void ResetLimitArrows()
    {
        foreach (GameObject arrow in limitArrows)
        {
            if (arrow.activeSelf)
                arrow.SetActive(false);
        }
    }
    public void SetLimitArrowActive(int arrowNumber, bool value)
    {
        if (limitArrows[arrowNumber].activeSelf != value)
            limitArrows[arrowNumber].SetActive(value);
    }

    #endregion

    #region Path Selection Canvas
    /// <summary>
    /// Set elements from Path Selection canvas innactive
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
    public void SetSelectionArrowActive(int arrowNumber, bool value)
    {
        if(selectionArrows[arrowNumber].activeSelf != value)
            selectionArrows[arrowNumber].SetActive(value);
    }

    public void SetPathSelectionTextActive(bool value)
    {
        if(PathSelectionText.activeSelf != value)
            PathSelectionText.SetActive(value);
    }
    #endregion

    #region Enemy Shield Bar Canvas
    public void SetEnemyShieldBarActive(bool value)
    {
        if(enemyShieldBar.activeSelf != value)
            enemyShieldBar.SetActive(value);
    }

    public void ResetEnemyShieldBar()
    {
        enemyShieldBarForeground.sizeDelta = new Vector2(500, 10);
    }

    public void SetEnemyShieldBarWidth(float width)
    {
        enemyShieldBarForeground.sizeDelta = new Vector2(width, enemyShieldBarForeground.sizeDelta.y);
    }

    public void SetEnemyShieldBarHeight(float height)
    {
        enemyShieldBarForeground.sizeDelta = new Vector2(enemyShieldBarForeground.sizeDelta.x, height);
    }
    #endregion

    #region Game Over Canvas
    /// <summary>
    /// Function that resets elements from Game Over canvas.
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
        gameOver.TransitionTo(0);
        GameManager.Instance.SetGameState(GameManager.StateType.Gameover);
        gameOverPanel.gameObject.SetActive(true);
        StartCoroutine("GameOverAnimation");
    }

    /// <summary>
    /// Actual animation for the GameOver screen
    /// </summary>
    private IEnumerator GameOverAnimation()
    {
        float t = 0.0f;
        var screenColor = gameOverPanel.color;
        var newScreenColor = screenColor;
        newScreenColor.a = 1f;
        score.text = "Score: " + GameManager.Instance.GetTotalScore();
        while (t < 1)
        {
            t += Time.deltaTime;
            gameOverPanel.color = Color.Lerp(screenColor, newScreenColor, t);
            yield return null;
        }
        Time.timeScale = 0f;
    }

    public void RetryLevel()
    {
        gamePlay.TransitionTo(0);
        GameManager.Instance.SetGameState(GameManager.StateType.Play);
        GameManager.Instance.ResetGameManager();
        GameManager.Instance.ResetScene();
    }
    #endregion

    public void PlaySoundClip(string name)
    {
        AudioManager.Instance.Play(name);
    }
}
