using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {

    #region SingletonAndAwake
    // Declare an instance of LevelManager.
    private static LevelManager instance = null;
    public static LevelManager Instance {
        get { return instance; }
    }

    private void Awake()
    {
        // If another instance already exist auto destroy and stop execution.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else { instance = this; }   // If the instance is not set, set this LevelManager as it.
    }
    #endregion

    public GameObject gameplayPlane;        // gameplayPlane GameObject.
    public Sector[] sectors;                // Array of sectors that form a Level.

    [Space(10)]
    [Header("Path Selection")]
    public GameObject limitPlane;           // limitPlane GameObject inside gamePlayplane.
    public float flickFrequency = 2.0f;     // Time it will take to flick an arrow.
    private bool pathSelection = false;     // If true, path selection will start.
    private bool arrowAnimFree = true;      // If the arrows are not being already animated.

    private GameObject player;              // GameObject of the Player.
    private Sector currentSector;           // Sector the player is currently in.
    private Sector nextSector;              // Sector that the player is reaching.
    private int currentSectorNumber;        // Index of the current sector.
    private bool canChangeSector = true;    // Bool to aboid trying to change sector while already changin one.
    private GameObject currentCamera;       // Camera that is currently in use.


    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");    // Set Player.
        currentSector = sectors[0];                             // Start in first sector.
        nextSector = sectors[0];                                // Set first sector, in case the Rail only contains one sector.
        currentSectorNumber = -1;                               // When start the first sector is the one that goes from the Rail GameObject to the first node.
        SetCurrentSectorPlayerMovement();                       // Let the Player move if it is set like that.
        SetCurrentSectorSpeed();                                // Set sector speed.
        if(currentSector.cameraChange)                          // Set start camera. If none was defined by default MainCamera is used.
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

        //Change rail orientation if needed.
        if (currentSector.railOrientation != gameplayPlane.GetComponent<RailMover>().orientationMode)
            SetCurrentSectorOrientation();
        // Show or hide boss shield bar.
        if (currentSector.showEnemyShieldbar)
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(true);
        else
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(false);
        // Loop throught same sector if needed.
        if (currentSector.loopSector)
            LoopSectorActive(true);
    }

    void LateUpdate()
    {
        // If the sector is not already changing, look for a change.
        if (canChangeSector)
            LookForSectorChange();
        // If Player is on a sector with two paths call functions for it.
        if (pathSelection)
            ChoosePath(currentSector.alternativeRail);
    }

    #region SectorFunctions

    /// <summary>
    /// Function that returns current sector.
    /// </summary>
    /// <returns> Current Sector.</returns>
    public Sector GetCurrentSector()
    {
        return currentSector;
    }

    /// <summary>
    /// Function that returns current sector speed.
    /// </summary>
    /// <returns> Speed of the current sector. </returns>
    public float GetCurrentSectorSpeed()
    {
        return currentSector.speed;
    }

    /// <summary>
    /// This function compares the first node of the next sector with the node that the player last passed through
    /// </summary>
    void LookForSectorChange()
    {
        if (gameplayPlane.GetComponent<RailMover>().GetCurrentNode().ToString() == nextSector.startNode.ToString()) // Uses string because could be the node of an alt rail.
        {
            canChangeSector = false;
            // Look for scene change.
            if (nextSector.changeScene) 
            {
                AudioManager.Instance.StopEverySound();
                GameManager.Instance.LoadScene(nextSector.sceneToLoad);
            }
            else
            {
                ChangeSectorToNext();                       // Change sector.
                if (currentSector.alternativeRail != null)  // Look if is a path selection sector.
                    ActivatePathSelection();
            }
        }
    }
    
    /// <summary>
    /// Function that makes every change nedeed between sectors.
    /// </summary>
    void ChangeSectorToNext()
    {
        // Before updating current sector look for rail orientation and mode change
        if (currentSector.railOrientation != nextSector.railOrientation)
            SetCurrentSectorOrientation();
        if (currentSector.railMode != nextSector.railMode && nextSector.alternativeRail == null)
            SetCurrentSectorRailMode();

        // Update sector order.
        currentSector = nextSector;
        currentSectorNumber++;

        // Change playerMovement.
        SetCurrentSectorPlayerMovement();

        // Change camera if needed.
        if (currentSector.cameraChange)
            SetCurrentSectorCamera();

        // Change speed.
        SetCurrentSectorSpeed();

        // Show boss shield bar if needed.
        if (currentSector.showEnemyShieldbar)
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(true);
        else
            PlayerHUDManager.Instance.SetEnemyShieldBarActive(false);

        // Loop throught same sector if needed.
        if (currentSector.loopSector)
            LoopSectorActive(true);

        // Change playing music if needed.
        if (currentSector.changeMusic)
            AudioManager.Instance.Play(sectors[currentSectorNumber].musicClipName);

        // Check if we reached last sector.
        if (sectors.Length - 1 > currentSectorNumber)
        {
            nextSector = sectors[currentSectorNumber+1];
        }
        else
        {
            // Reset values just in case it loops.
            currentSectorNumber = 0;
            currentSector = sectors[0];
            if (sectors.Length > 1)
                nextSector = sectors[1];
        }
        canChangeSector = true;
    }

    /// <summary>
    /// Function that changes speed to current sector speed.
    /// </summary>
    void SetCurrentSectorSpeed()
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().speed = currentSector.speed;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    /// <summary>
    /// Function that changes camera to current sector camera.
    /// </summary>
    void SetCurrentSectorCamera()
    {
        currentCamera.gameObject.SetActive(false);
        currentCamera = currentSector.newCamera.gameObject;
        currentCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// Function that enables or disables Player controll on the ship.
    /// </summary>
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

    /// <summary>
    /// Function that sets Orientation on the Rail to the orientation of the sector.
    /// </summary>
    void SetCurrentSectorOrientation()
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().orientationMode = sectors[currentSectorNumber+1].railOrientation;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    /// <summary>
    /// Function that sets RailMode of the Rail to the mode of the sector.
    /// </summary>
    void SetCurrentSectorRailMode()
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().playMode = sectors[currentSectorNumber + 1].railMode;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    /// <summary>
    /// Function that will set loop over current sector active or not.
    /// </summary>
    /// <param name="loop"> If true current sector will be looped.</param>
    public void LoopSectorActive(bool loop)
    {
        gameplayPlane.GetComponent<RailMover>().loopNode = loop;
    }

    #endregion

    #region LevelManagementFunctions

    /// <summary>
    /// Pauses the level speed and disables boost or brake.
    /// </summary>
    public void PauseLevel()
    {
        player.GetComponent<ShipController>().BlockBoost(true);
        gameplayPlane.GetComponent<RailMover>().speed = 0;
    }

    /// <summary>
    /// Sets Level speed back to normal and enables boost or brake.
    /// </summary>
    public void ContinueLevel()
    {
        gameplayPlane.GetComponent<RailMover>().speed = currentSector.speed;
        player.GetComponent<ShipController>().BlockBoost(false);
    }
    
    /// <summary>
    /// Function to manage the GameOver screen.
    /// </summary>
    public void LevelGameOver()
    {

        AudioManager.Instance.StopEverySound();
        PlayerHUDManager.Instance.ShowGameOverScreen();
    }
    #endregion

    #region PathSelectionFunctions

    /// <summary>
    /// Funtion that manages Path Selection Sectors. This means that there are two paths available to the player to choose from.
    /// When the Player has advanced enought on the sector the selection is made, this selection is based on the position of the player on the gameplayPlane.
    /// </summary>
    /// <param name="newRail"> Rail that will be an alternative selection for the Player.</param>
    public void ChoosePath(Rail newRail)
    {
        var position = limitPlane.GetComponent<PlayerLimitManager>().GetPlayerLocationInPlane(currentSector.divideType);
        //Animation of arrows depending on the position of the player and divideType.
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
        // Look for end of segment, to apply selection.
        if (gameplayPlane.GetComponent<RailMover>().GetPositionOnSegment() > 0.9f)
        {
            pathSelection = false;
            // If the player is down or right (depends on the divideType) respect to the gameplayPlane, change to a new Rail.
            if (position == "down" || position == "right")
                ChangeToAlternativeRail();
        }
    }

    /// <summary>
    /// Function that sets Rail of the RailMover script the alternative Rail offered.
    /// </summary>
    private void ChangeToAlternativeRail()
    {
        gameplayPlane.GetComponent<RailMover>().rail = currentSector.alternativeRail;
    }

    /// <summary>
    /// Function that sets Rail of the RailMover script the one given as a parameter.
    /// </summary>
    private void SetCureentRail(Rail rail)
    {
        gameplayPlane.GetComponent<RailMover>().rail = rail;
    }

    /// <summary>
    /// Animation of the arrows from Player HUD. The arrow passed as parameter will flick once.
    /// </summary>
    /// <param name="currentArrow"> Arrow to be animated.</param>
    private IEnumerator ArrowAnimation(int currentArrow)
    {
        arrowAnimFree = false;
        PlayerHUDManager.Instance.SetSelectionArrowActive(currentArrow, true);
        yield return new WaitForSeconds(flickFrequency);
        PlayerHUDManager.Instance.SetSelectionArrowActive(currentArrow, false);
        yield return new WaitForSeconds(flickFrequency);
        arrowAnimFree = true;
    }

    /// <summary>
    /// Animation for the text that shows up so the Player knows he can choose a path.
    /// It will flick untill enought part of the segment has been traveled.
    /// </summary>
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

    /// <summary>
    /// Function that activates the path selection functions.
    /// </summary>
    private void ActivatePathSelection()
    {
        pathSelection = true;
        StartCoroutine("TextAnimation");
    }
    #endregion
}
