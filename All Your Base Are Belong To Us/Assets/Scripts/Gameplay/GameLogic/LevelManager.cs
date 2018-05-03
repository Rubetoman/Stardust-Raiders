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
    public class Sector
    {
        public GameObject[] nodes;
        public float speed;
        public Camera camera;
        public GameObject[] enemies;
    }

    public Sector[] sectors;
    public GameObject gameplayPlane;
    private Sector currentSector;
    private Sector nextSector;
    private int currentSectorNumber;
    private bool canChangeSector = true;
    private GameObject currentCamera;
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
	void Update () {
        LookForSectorChange();

    }

    /// <summary>
    /// This function compares the first node of the netx sector with the node that i
    /// </summary>
    void LookForSectorChange()
    {
        if (gameplayPlane.GetComponent<RailMover>().getCurrentNode() == nextSector.nodes[0] && canChangeSector)
        {
            canChangeSector = false;
            ChangeSectorToNext();
        }
    }

    void ChangeSectorToNext()
    {
        print(currentSectorNumber);
        currentSector = nextSector;
        currentSectorNumber++;
        if (sectors.Length-1 > currentSectorNumber)
        {
            nextSector = sectors[currentSectorNumber];
            canChangeSector = true;
            if (currentSector.camera != null)
                ChangeToCurrentSectorCamera();
            SetSpeed(sectors[currentSectorNumber].speed);
        }
    }

    void SetSpeed(float speed)
    {
        if (gameplayPlane.GetComponent<RailMover>() != null)
            gameplayPlane.GetComponent<RailMover>().speed = speed;
        else
            Debug.LogError("RailMover Script couldn't be found inside gameplayPlane GameObject");
    }

    void ChangeToCurrentSectorCamera()
    {
        print("b");
        currentCamera.gameObject.SetActive(false);
        currentCamera = currentSector.camera.gameObject;
        currentCamera.gameObject.SetActive(true);
    }

    void SetEnemiesActive()
    {
        //sectors+[]
    }
}
