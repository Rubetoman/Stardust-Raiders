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

    public GameObject gameplayPlane;    // gameplayPlane GameObject
    public Sector[] sectors;            // Array of sectors that form a Level

    [Space(10)]
    [Header("Path Selection")]
    public GameObject limitPlane;
    public float flickFrequency = 2.0f; // Time it will take to flick an arrow
    private bool pathSelection = false;
    private bool arrowAnimFree = true;

    private GameObject player;          // GameObject of the Player
    private Sector currentSector;       // Sector the player is currently in
    private Sector nextSector;          // Sector that the player is reaching
    private int currentSectorNumber;    // Index of the current sector
    private bool canChangeSector = true;// Bool to aboid trying to change sector while already changin one
    private GameObject currentCamera;   // Camera that is currently in use
    private string position;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        currentSector = sectors[0];
        nextSector = sectors[0];
        currentSectorNumber = -1;
        SetCurrentSectorPlayerMovement();
        SetCurrentSectorSpeed();
        if(currentSector.cameraChange)
        {
            currentCamera = currentSector.newCamera.gameObject;
            currentCamera.SetActive(true);
            if(currentCamera != GameObject.FindGameObjectWithTag("MainCamera"))
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().gameObject.SetActive(false);
        }
        else
        {
            currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().gameObject;
        }
        //mainRail = gameplayPlane.GetComponent<RailMover>().rail;
        //Change rail orientation if needed
        if (currentSector.railOrientation != gameplayPlane.GetComponent<RailMover>().orientationMode)
            SetCurrentSectorOrientation();
        // Show or hide boss shield bar
        if (currentSector.showEnemyShieldbar)
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(true);
        else
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(false);
        // Loop throught same sector if needed
        if (currentSector.loopSector)
            LoopSectorActive(true);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (canChangeSector)
            LookForSectorChange();
        if (pathSelection)
        {
            ChoosePath(currentSector.alternativeRail);
        }
    }

    #region SectorFunctions

    public Sector GetCurrentSector()
    {
        return currentSector;
    }
    /// <summary>
    /// This function compares the first node of the next sector with the node that the player last passed through
    /// </summary>
    void LookForSectorChange()
    {
        if (gameplayPlane.GetComponent<RailMover>().GetCurrentNode().ToString() == nextSector.startNode.ToString()) // Uses string because could be the node of an alt rail
        {
            canChangeSector = false;
            if (nextSector.changeScene)
            {
                AudioManager.Instance.StopSound();
                GameManager.Instance.LoadScene(nextSector.sceneToLoad);
            }
            else
            {
                ChangeSectorToNext();
                if (currentSector.alternativeRail != null)
                    ActivatePathSelection();
            }
        }
    }
    /// <summary>
    /// Function that makes every change nedeed between sectors
    /// </summary>
    void ChangeSectorToNext()
    {
        // Before updating current sector look for rail orientation and mode change
        if (currentSector.railOrientation != nextSector.railOrientation)
            SetCurrentSectorOrientation();
        if (currentSector.railMode != nextSector.railMode && nextSector.alternativeRail == null)
            SetCurrentSectorRailMode();
        // Update sector order
        currentSector = nextSector;
        currentSectorNumber++;
        // Change playerMovement
        SetCurrentSectorPlayerMovement();
        // Change camera if needed
        if (currentSector.cameraChange)
            SetCurrentSectorCamera();
        // Change speed
        SetCurrentSectorSpeed();
        // Show boss shield bar if needed
        if (currentSector.showEnemyShieldbar)
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(true);
        else
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(false);

        // Loop throught same sector if needed
        if (currentSector.loopSector)
            LoopSectorActive(true);

        // Change playing music if needed
        if (currentSector.changeMusic)
            AudioManager.Instance.Play(sectors[currentSectorNumber].musicClipName);

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
            gameplayPlane.GetComponent<RailMover>().speed = currentSector.speed;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    /// <summary>
    /// Change camera to current sector camera
    /// </summary>
    void SetCurrentSectorCamera()
    {
        currentCamera.gameObject.SetActive(false);
        currentCamera = currentSector.newCamera.gameObject;
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

    void SetCurrentSectorOrientation()
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().orientationMode = sectors[currentSectorNumber+1].railOrientation;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    void SetCurrentSectorRailMode()
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().playMode = sectors[currentSectorNumber + 1].railMode;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    public void LoopSectorActive(bool loop)
    {
        // Make it loop throught the same sector
        gameplayPlane.GetComponent<RailMover>().loopNode = loop;
    }
    #endregion

    #region LevelManagementFunctions
    /// <summary>
    /// Pauses the level speed
    /// </summary>
    public void PauseLevel()
    {
        player.GetComponent<ShipController>().BlockBoost(true);
        gameplayPlane.GetComponent<RailMover>().speed = 0;
    }

    /// <summary>
    /// Sets Level speed back to normal
    /// </summary>
    public void ContinueLevel()
    {
        gameplayPlane.GetComponent<RailMover>().speed = currentSector.speed;
        player.GetComponent<ShipController>().BlockBoost(false);
    }
    
    /// <summary>
    /// Function to manage the GameOver screen
    /// </summary>
    public void LevelGameOver()
    {
        AudioManager.Instance.StopSound();
        PlayerHUDManager.Instance.ShowGameOverScreen();
    }
    #endregion

    #region PathSelectionFunctions
    public void ChoosePath(Rail newRail)
    {
        var position = limitPlane.GetComponent<PlayerLimitManager>().GetPlayerLocationInPlane(currentSector.divideType);
        //Animation of arrows
        switch (position)
        {
            case "up":
                if (arrowAnimFree)
                    StartCoroutine("ArrowAnimation", 0);
                break;
            case "down":
                if (arrowAnimFree)
                    StartCoroutine("ArrowAnimation", 1);
                break;
            case "left":
                if (arrowAnimFree)
                    StartCoroutine("ArrowAnimation", 2);
                break;
            case "right":
                if (arrowAnimFree)
                    StartCoroutine("ArrowAnimation", 3);
                break;
        }
        // Look for end of segment, to apply selection   
        if (gameplayPlane.GetComponent<RailMover>().GetPositionOnSegment() > 0.9f)
        {
            pathSelection = false;
            if (position == "down" || position == "right")
                ChangeToAlternativeRail();
        }
    }

    private void ChangeToAlternativeRail()
    {
        gameplayPlane.GetComponent<RailMover>().rail = currentSector.alternativeRail;
    }

    private void SetCureentRail(Rail rail)
    {
        gameplayPlane.GetComponent<RailMover>().rail = rail;
    }

    private IEnumerator ArrowAnimation(int currentArrow)
    {
        arrowAnimFree = false;
        PlayerHUDManager.Instance.SetSelectionArrowActive(currentArrow, true);
        yield return new WaitForSeconds(flickFrequency);
        PlayerHUDManager.Instance.SetSelectionArrowActive(currentArrow, false);
        yield return new WaitForSeconds(flickFrequency);
        arrowAnimFree = true;
    }

    private IEnumerator TextAnimation()
    {
        while (gameplayPlane.GetComponent<RailMover>().GetPositionOnSegment() < 0.4f)
        {
            PlayerHUDManager.Instance.SetPathSelectionTextActive(true);
            yield return new WaitForSeconds(flickFrequency);
            PlayerHUDManager.Instance.SetPathSelectionTextActive(false);
            yield return new WaitForSeconds(flickFrequency);
        }
    }

    private void ActivatePathSelection()
    {
        pathSelection = true;
        StartCoroutine("TextAnimation");
    }
    #endregion
}
