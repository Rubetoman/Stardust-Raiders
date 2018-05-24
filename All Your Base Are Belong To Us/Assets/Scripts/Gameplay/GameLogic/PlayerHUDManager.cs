using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour {
    #region SingletonAndAwake
    private static PlayerHUDManager _instance;
    public static PlayerHUDManager Instance {
        get {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<PlayerHUDManager>();
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
    public Text lifesCount;
    [Space(10)]
    [Header("Shield Bar Canvas")]
    public RectTransform playerShieldBarForeground;
    [Space(10)]
    [Header("Boost Bar Canvas")]
    public RectTransform boostBarForeground;
    [Space(10)]
    [Header("Path Selection Canvas")]
    public GameObject[] arrows;         // The UI arrows which appear pointing both paths. Must be inserted in the following order: (up, low, left, right).
    public GameObject PathSelectionText;
    [Space(10)]
    [Header("Enemy Shield Canvas")]
    public GameObject enemyShieldBar;
    public RectTransform enemyShieldBarForeground;
    [Space(10)]
    [Header("Game Over Canvas")]
    public Image gameOverScreen;        // Background Image of the Game Over Screen
    public Text gameOverText;           // Text to be shown on Game Over
    public Text score;                  // Text tha will show the Total Score on the Game Over Screen

    
    #region Shield Bar Canvas
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
    public void SetBoostBarWidth(float width)
    {
        boostBarForeground.sizeDelta = new Vector2(width, boostBarForeground.sizeDelta.y);
    }

    public void SetBoostBarHeight(float height)
    {
        boostBarForeground.sizeDelta = new Vector2(boostBarForeground.sizeDelta.x, height);
    }
    #endregion

    #region Path Selection Canvas
    public void SetArrowActive(int arrowNumber, bool value)
    {
        if(arrows[arrowNumber].activeSelf != value)
            arrows[arrowNumber].SetActive(value);
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
    /// Function to manage the GameOver screen
    /// </summary>
    public void ShowGameOverScreen()
    {
        score.gameObject.SetActive(true);
        StartCoroutine("GameOverAnimation");
    }

    /// <summary>
    /// Actual animation for the GameOver screen
    /// </summary>
    private IEnumerator GameOverAnimation()
    {
        float t = 0.0f;
        var screenColor = gameOverScreen.color;
        var newScreenColor = screenColor;
        newScreenColor.a = 1f;
        var textColor = gameOverText.color;
        var newTextColor = textColor;
        newTextColor.a = 1f;

        while (t < 1)
        {
            t += Time.deltaTime;
            gameOverScreen.color = Color.Lerp(screenColor, newScreenColor, t);
            gameOverText.color = Color.Lerp(textColor, newTextColor, t);
            yield return null;
        }
        score.text = "Score: " + GameManager.Instance.GetTotalScore();
    }
    #endregion
}
