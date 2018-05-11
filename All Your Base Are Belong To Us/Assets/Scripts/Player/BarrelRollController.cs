using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRollController : MonoBehaviour {

    public float multipleTapDelay = 1.0f;   // Max delay between taps of a Input to be considered as consecutive taps
    public float barrelRollDuration = 1.0f; // Time it takes to do the barrel roll
    public float maxBankAngle = 85f;        // The maximun degrees we want our starship to bank (Euler Angles)
    public GameObject playerMainBody;
    public GameObject barrelRollShield;

    private bool buttonDown = false;
    private float leftTimer = 1.0f;
    private float rightTimer = 1.0f;

    [HideInInspector]
    public bool inBarrelRoll = false;


    private void Start()
    {
        if (playerMainBody.GetComponent<ShieldManager>() == null)
            Debug.LogError("Shield Manager not found on the player main body");
    }
    // Update is called once per frame
    void Update()
    {
        if (!inBarrelRoll)
        {
            Bank("Bank", maxBankAngle);
        }
        
    }
    /// <summary>
    /// The function to make the ship bank
    /// </summary>
    /// <param name="axis"> Axis which will make the ship bank</param>
    /// <param name="bankAngle"> How much degrees is going to bank</param>
    public void BankNoBarrelRoll(string axis, float bankAngle)
    {
        float bankAxis = Input.GetAxis(axis); //Get input
        //Banking
        Quaternion startRotation = transform.localRotation;
        Quaternion newRotation = transform.localRotation;
        //Calculate the new Z rotation, bankAxis will make the maximun bank angle to be at full or not
        //maxBankAngle is divided by 10 because is in Euler Angles and we want Quaternion angles
        newRotation.z = (startRotation * Quaternion.AngleAxis(bankAxis * (-bankAngle / 10), Vector3.forward)).z;
        transform.localRotation = newRotation;
    }

    /// <summary>
    /// The function to make the ship bank. Also calls the BarrelRoll function if the button press is fast enought
    /// </summary>
    /// <param name="axis"> Axis which will make the ship bank</param>
    /// <param name="bankAngle"> How much degrees is going to bank</param>
    public void Bank(string axis, float bankAngle)
    {
        float bankAxis = Input.GetAxis(axis); //Get input
        //Banking
        Quaternion startRotation = transform.localRotation;
        Quaternion newRotation = transform.localRotation;
        //Calculate the new Z rotation, bankAxis will make the maximun bank angle to be at full or not
        //maxBankAngle is divided by 10 because is in Euler Angles and we want Quaternion angles
        newRotation.z = (startRotation * Quaternion.AngleAxis(bankAxis * (-bankAngle / 10), Vector3.forward)).z;
        transform.localRotation = newRotation;

        //BarrelRoll detection
        if (bankAxis == 0.0f)
        {
            buttonDown = false;
        }
        else if (buttonDown == false)
        {
            if (bankAxis < 0.0f)
            {
                buttonDown = true;
                if (leftTimer < multipleTapDelay)
                {
                    StartCoroutine("BarrelRoll", -1);
                }
                else
                {
                    leftTimer = 0.0f;
                }
            }
            else if (bankAxis > 0.0f)
            {
                buttonDown = true;
                if (rightTimer < multipleTapDelay)
                {
                    StartCoroutine("BarrelRoll", 1);
                }
                rightTimer = 0.0f;
            }

        }
        leftTimer += Time.deltaTime;
        rightTimer += Time.deltaTime;
    }

    void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// When the function is called it makes the spaceship to do a barrelRoll to the side passed as parameter
    /// If side is -1 it means is rolling to the left, else it goes to the right
    /// </summary>
    private IEnumerator BarrelRoll(int side)
    {

        inBarrelRoll = true;
        playerMainBody.GetComponent<PlayerShieldManager>().inBarrelRoll = true;

        float t = 0.0f;
        Quaternion startRot = transform.rotation;
        barrelRollShield.SetActive(true);
        while (t < barrelRollDuration)
        {
            t += Time.deltaTime;
            barrelRollShield.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(6.5f, 6.5f, 6.5f), t / barrelRollDuration );
            if (side < 0.0f)
                transform.rotation = startRot * Quaternion.AngleAxis(t / barrelRollDuration * 360f, Vector3.forward); //or transform.right if you want it to be locally based
            else
                transform.rotation = startRot * Quaternion.AngleAxis(t / barrelRollDuration * 360f, -Vector3.forward);
            
            yield return null;
        }
        barrelRollShield.SetActive(false);
        transform.rotation = startRot;
        inBarrelRoll = false;
        playerMainBody.GetComponent<PlayerShieldManager>().inBarrelRoll = false;
    }
}
