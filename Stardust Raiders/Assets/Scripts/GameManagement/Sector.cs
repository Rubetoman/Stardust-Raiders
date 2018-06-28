using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of division the sector has. If the player can choos between up/down or left/right paths.
/// </summary>
public enum DivideType
{
    Up_Down,
    Left_Right,
}

/// <summary>
/// Script to define each sector caracteristics.
/// </summary>
[System.Serializable]
public class Sector : MonoBehaviour
{
    // This booleans are only used for the Editor
    public bool cameraChange = false;       // If true the cameraChange variable field will show on inspector.
    public bool pathSelection;              // If true the alternativeRail and divideType variables fields will show on inspector.
    public bool changeScene = false;        // If true the sceneToLoad variable will show.
    public bool changeMusic;                // If true the musicClipName variable will show

    // Each sector properties
    public GameObject startNode;            // First node of the sector.
    public float speed;                     // Speed of the playerShip inside the sector.
    public Camera newCamera;                // Camera to change in case this sector has a different camera from the previous sector.
    public bool playerMovement = true;      // If true the player controls are active, else they are disabled .
    public Rail alternativeRail;            // Second rail to choose between the one already in use and this one.
    public DivideType divideType;           // Type of division on the path selection.
    public OrientationMode railOrientation; // Choose between orientation of the object on the rail (Node: use same rotation of the nodes, Lines: look at next node).
    public PlayMode railMode;
    public string sceneToLoad;              // At the end of this sector the scene with that name will be loaded.
    public bool loopSector = false;         // If true in this scene there will be a boss fight.
    public bool showEnemyShieldbar = false; // If true will display enemy shield bar on player HUD.
    public string musicClipName;            // Name of the song to play (It must be added to the Audio Manager)
}
