using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    #region SingletonAndAwake
    private static LevelManager instance = null;
    public static LevelManager Instance {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [System.Serializable]
    public class Sector             // Each sector contains a set of variables
    {
        public GameObject startNode;        // First node of the sector
        public float speed;                 // Speed of the playerShip inside the sector
        public Camera camera;               // Camera to change in case this sector has a different camera from the previous sector
        public bool playerMovement = true;  // If true the player controls are active, else they are disabled 
        public Rail alternativeRail;        // Second rail to choose between the one already in use and this one
        public bool changeScene = false;    // If true, at the end of this sector a new scene will be loaded
    }

    public Sector[] sectors;            // Array of sectors that form a Level
    public GameObject gameplayPlane;    // gameplayPlane GameObject
    public Image gameOverScreen;        // Background Image of the Game Over Screen
    public Text gameOvertext;           // Text to be shown on Game Over
    public Text score;                  // Text tha will show the Total Score on the Game Over Screen

    private GameObject player;          // GameObject of the Player
    private Sector currentSector;       // Sector the player is currently in
    private Sector nextSector;          // Sector that the player is reaching
    private int currentSectorNumber;    // Index of the current sector
    private bool canChangeSector = true;// Bool to aboid trying to change sector while already changin one
    private GameObject currentCamera;   // Camera that is currently in use

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        currentSector = sectors[0];
        if(sectors.Length>1)
            nextSector = sectors[1];
        currentSectorNumber = 0;
        SetCurrentSectorPlayerMovement();
        SetCurrentSectorSpeed();
        if(currentSector.camera != null)
        {
            currentCamera = currentSector.camera.gameObject;
            currentCamera.SetActive(true);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().gameObject.SetActive(false);
        }
        else
        {
            currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().gameObject;
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (canChangeSector)
            LookForSectorChange();
    }

    #region SectorFunctions
    /// <summary>
    /// This function compares the first node of the next sector with the node that the player last passed through
    /// </summary>
    void LookForSectorChange()
    {
        if (gameplayPlane.GetComponent<RailMover>().getCurrentNode() == nextSector.startNode)
        {
            canChangeSector = false;
            if (currentSector.changeScene)
            {
                GameManager.Instance.SetScene("boss_fight");
            }
            else
            {
                ChangeSectorToNext();
                if (currentSector.alternativeRail != null)
                    StartPathSelection();
            }
        }
    }
    /// <summary>
    /// Function that makes every change nedeed between sectors
    /// </summary>
    void ChangeSectorToNext()
    {
        // Update sector order
        currentSector = nextSector;
        currentSectorNumber++;
        //Change playerMovement
        SetCurrentSectorPlayerMovement();
        //Change camera if needed
        if (currentSector.camera != null)
            SetCurrentSectorCamera();
        //Change speed
        SetCurrentSectorSpeed();

        // Check if we reached last sector
        if (sectors.Length - 1 > currentSectorNumber)
        {
            nextSector = sectors[currentSectorNumber+1];
        }
        else
        {
            // Reset values just in case it loops
            currentSectorNumber = 0;
            currentSector = sectors[0];
            if (sectors.Length > 1)
                nextSector = sectors[1];
        }
        canChangeSector = true;
    }

    /// <summary>
    /// Change speed to current sector speed
    /// </summary>
    void SetCurrentSectorSpeed()
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().speed = sectors[currentSectorNumber].speed;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    /// <summary>
    /// Change camera to current sector camera
    /// </summary>
    void SetCurrentSectorCamera()
    {
        currentCamera.gameObject.SetActive(false);
        currentCamera = currentSector.camera.gameObject;
        currentCamera.gameObject.SetActive(true);
    }

    void SetCurrentSectorPlayerMovement()
    {
        var state = currentSector.playerMovement;
        if(state != player.GetComponent<ShipController>().enabled)
        {
            player.GetComponent<ShipController>().enabled = state;
            player.GetComponent<BarrelRollController>().enabled = state;
            player.GetComponent<PlayerGunController>().enabled = state;
            player.GetComponentInChildren<PlayerShieldManager>().enabled = state;
        }
    }
    #endregion

    #region LevelManagementFunctions
    /// <summary>
    /// Pauses the level speed
    /// </summary>
    public void PauseLevel()
    {
        gameplayPlane.GetComponent<RailMover>().speed = 0;
    }

    /// <summary>
    /// Sets Level speed back to normal
    /// </summary>
    public void ContinueLevel()
    {
        gameplayPlane.GetComponent<RailMover>().speed = currentSector.speed;
    }
    
    /// <summary>
    /// Function to manage the GameOver screen
    /// </summary>
    public void LevelGameOver()
    {
        score.gameObject.SetActive(true);
        StartCoroutine("GameOverAnimation");
        GameManager.Instance.Invoke("ResetScene", 10);
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
        var textColor = gameOvertext.color;
        var newTextColor = textColor;
        newTextColor.a = 1f;

        while (t < 1)
        {
            t += Time.deltaTime;
            gameOverScreen.color = Color.Lerp(screenColor, newScreenColor, t);
            gameOvertext.color = Color.Lerp(textColor, newTextColor, t);
            yield return null;
        }
        score.text = "Score: " + GameManager.Instance.GetTotalScore();
    }
    #endregion

    void StartPathSelection()
    {
        GetComponent<PathDivider>().activatePathSelection(currentSector.alternativeRail);
    }

}
