using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public GameObject[] nodes;  // Nodes that form a sector
        public float speed;         // Speed of the playerShip inside the sector
        public Camera camera;       // Camera to change in case this sector has a different camera from the previous sector
        public bool playerMovement; // If true the player controls are active, else they are disabled 
        public GameObject[] enemies;// List of enemies that would be spawned
    }

    public Sector[] sectors;            // Array of sectors that form a Level
    public GameObject gameplayPlane;    //

    private Sector currentSector;       // Sector the player is currently in
    private Sector nextSector;          // Ssector that the player is reaching
    private int currentSectorNumber;    // Index of the current sector
    private bool canChangeSector = true;//
    private GameObject currentCamera;   // Camera that is currently in use

    // Use this for initialization
    void Start () {
        currentSector = sectors[0];
        if(sectors.Length>1)
            nextSector = sectors[1];
        currentSectorNumber = 0;
        SetSpeed(currentSector.speed);
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

    /// <summary>
    /// This function compares the first node of the next sector with the node that the player last passed through
    /// </summary>
    void LookForSectorChange()
    {
        if (gameplayPlane.GetComponent<RailMover>().getCurrentNode() == nextSector.nodes[0])
        {
            canChangeSector = false;
            ChangeSectorToNext();
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
        //Cange camera if needed
        if (currentSector.camera != null)
            ChangeToCurrentSectorCamera();
        //Change speed
        SetSpeed();

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
    void SetSpeed()
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().speed = sectors[currentSectorNumber].speed;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    /// <summary>
    /// Change camera to current sector camera
    /// </summary>
    void ChangeToCurrentSectorCamera()
    {
        currentCamera.gameObject.SetActive(false);
        currentCamera = currentSector.camera.gameObject;
        currentCamera.gameObject.SetActive(true);
    }

    void SetEnemiesActive()
    {
        //sectors+[]
    }
}
