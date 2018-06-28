using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script to controll banking and barrel roll movement of the Player's Ship.
/// </summary>
public class BarrelRollController : MonoBehaviour {

    public float multipleTapDelay = 1.0f;   // Max delay between taps of a Input to be considered as consecutive taps.
    public float barrelRollDuration = 1.0f; // Time it takes to do the barrel roll.
    public float maxBankAngle = 85f;        // The maximun degrees to bank the ship (Euler Angles).
    public GameObject playerMainBody;       // Player's main body (PlayerCollider).
    public GameObject barrelRollShield;     // The shield like GameObject that will be activate when doing a barrel roll.

    private bool buttonDown = false;        // Boolean to know when the bank button is pressed.
    private float leftTimer = 1.0f;         // Timer for the left bank Input.
    private float rightTimer = 1.0f;        // Timer for the right bank Input.

    [HideInInspector]
    public bool inBarrelRoll = false;       // Boolean that tels if the ship is performing a barrel roll or not.


    private void Start()
    {
        // Get Player's shield script.
        if (playerMainBody.GetComponent<ShieldManager>() == null)
            Debug.LogError("Shield Manager not found on the player main body");
    }

    void Update()
    {
        // If the Player is not performing a barrel roll or outside Play mode, let him bank and look for barrel roll Input.
        if (!inBarrelRoll && GameManager.Instance.gameState == GameManager.StateType.Play)
            Bank("Bank", maxBankAngle);
        
    }
    /// <summary>
    /// The function to make the ship bank.
    /// </summary>
    /// <param name="axis"> Axis which will make the ship bank.</param>
    /// <param name="bankAngle"> How much degrees the ship is going to bank.</param>
    public void BankNoBarrelRoll(string axis, float bankAngle)
    {
        float bankAxis = Input.GetAxis(axis); // Get input.
        Quaternion newRotation = transform.localRotation;
        // Calculate the new Z rotation, bankAxis will make the maximun bank angle to be at full or not.
        // maxBankAngle is divided by 10 because is in Euler Angles and we want Quaternion angles.
        newRotation.z = (transform.localRotation * Quaternion.AngleAxis(bankAxis * (-bankAngle / 10), Vector3.forward)).z;
        transform.localRotation = newRotation;
    }

    /// <summary>
    /// The function to make the ship bank. Also calls the BarrelRoll function if the button press is fast enought.
    /// </summary>
    /// <param name="axis"> Axis which will make the ship bank.</param>
    /// <param name="bankAngle"> How much degrees is going to bank.</param>
    public void Bank(string axis, float bankAngle)
    {
        float bankAxis = Input.GetAxis(axis); // Get input.
        Quaternion newRotation = transform.localRotation;

        // Calculate the new Z rotation, bankAxis will make the maximun bank angle to be at full or not.
        // maxBankAngle is divided by 10 because is in Euler Angles and we want Quaternion angles.
        newRotation.z = (transform.localRotation * Quaternion.AngleAxis(bankAxis * (-bankAngle / 10), Vector3.forward)).z;
        transform.localRotation = newRotation;

        // BarrelRoll detection.
        if (bankAxis == 0.0f)                       // There is no bank Input.
        {
            buttonDown = false;
        }
        else if (buttonDown == false)               // There is bank Input coming from a 0 value.
        {
            if (bankAxis < 0.0f)                    // Left bank Input.
            {
                buttonDown = true;
                if (leftTimer < multipleTapDelay)   // The time between two bank button press was fast enought. Perform barrel roll.
                {
                    StartCoroutine("BarrelRoll", -1);
                }                                
                leftTimer = 0.0f;                   // Reset left timer.
            }
            else if (bankAxis > 0.0f)               // Right bank Input.
            {
                buttonDown = true;
                if (rightTimer < multipleTapDelay)  // The time between two bank button press was fast enought. Perform barrel roll.
                {
                    StartCoroutine("BarrelRoll", 1);
                }
                rightTimer = 0.0f;                  // Reset right timer.
            }

        }
        leftTimer += Time.deltaTime;
        rightTimer += Time.deltaTime;
    }

    /// <summary>
    /// Funtion to set Player's ship rotation back to 0.
    /// </summary>
    void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Funtion to reset Player's barrel roll shield.
    /// </summary>
    public void ResetBarrelRollShield()
    {
        barrelRollShield.transform.localScale = Vector3.zero;
        barrelRollShield.SetActive(false);
    }

    /// <summary>
    /// When the function is called it makes the spaceship to do a barrelRoll to the side passed as parameter.
    /// If side is -1 it means is rolling to the left, else it goes to the right.
    /// It will activate a shield GameObject and animate it. While executing a barrel roll; Player doesn't take damage from bullets.
    /// </summary>
    private IEnumerator BarrelRoll(int side)
    {
        AudioManager.Instance.Play("BarrelRoll");   // Play sound effect.
        inBarrelRoll = true;
        playerMainBody.GetComponent<PlayerShieldManager>().inBarrelRoll = true;

        float t = 0.0f;
        Quaternion startRot = transform.rotation;
        barrelRollShield.SetActive(true);
        while (t < barrelRollDuration)
        {
            t += Time.deltaTime;
            barrelRollShield.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(6.5f, 6.5f, 6.5f), t / barrelRollDuration ); // Increase barrel roll shield scale with time.
            if (side < 0.0f)
                transform.rotation = startRot * Quaternion.AngleAxis(t / barrelRollDuration * 360f, Vector3.forward);                   // Right barrel roll.
            else
                transform.rotation = startRot * Quaternion.AngleAxis(t / barrelRollDuration * 360f, -Vector3.forward);                  // Left barrel roll.
            yield return null;
        }
        barrelRollShield.SetActive(false);
        transform.rotation = startRot;
        inBarrelRoll = false;
        playerMainBody.GetComponent<PlayerShieldManager>().inBarrelRoll = false;
    }
}
