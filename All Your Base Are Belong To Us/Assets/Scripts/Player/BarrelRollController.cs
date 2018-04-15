using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRollController : MonoBehaviour {

    public float multipleTapDelay = 1.0f; //Max delay between taps of a Input to be considered as consecutive taps
    public float barrelRollDuration = 1.0f; //Time it takes to do the barrel roll
    public float maxBankAngle = 85f; //The maximun degrees we want our starship to bank (Euler Angles)
    public bool inBarrelRoll = false;

    private bool buttonDown = false;
    private float leftTimer = 1.0f;
    private float rightTimer = 1.0f;

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

    /// <summary>
    /// When the function is called it makes the spaceship to do a barrelRoll to the side passed as parameter
    /// If side is -1 it means is rolling to the left, else it goes to the right
    /// </summary>
    private IEnumerator BarrelRoll(int side)
    {
        inBarrelRoll = true;

        float t = 0.0f;
        Quaternion startRot = transform.rotation;
        while (t < barrelRollDuration)
        {
            t += Time.deltaTime;
            if (side < 0.0f)
                transform.rotation = startRot * Quaternion.AngleAxis(t / barrelRollDuration * 360f, Vector3.forward); //or transform.right if you want it to be locally based
            else
                transform.rotation = startRot * Quaternion.AngleAxis(t / barrelRollDuration * 360f, -Vector3.forward);
            yield return null;
        }
        transform.rotation = startRot;
        inBarrelRoll = false;
    }




    /// <summary>
    /// Old BarrelRoll function
    /// </summary>
    /*private IEnumerator BarrelRoll(int side)
    {
        inBarrelRoll = true;
        float t = 0.0f;
        Vector3 initialRot = transform.localRotation.eulerAngles;
        Vector3 currentRot = initialRot;
        Vector3 goalRot = initialRot;
        if (side < 0.0f)
            goalRot.z += 180.0f;
        else if(side > 0.0f)
            goalRot.z -= 180.0f;
        

        while (t < barrelRollDuration / 2.0f)
        {
            currentRot.z = Mathf.Lerp(initialRot.z, goalRot.z, t / (barrelRollDuration / 2.0f));
            transform.localRotation = Quaternion.Euler(currentRot);
            t += Time.deltaTime;
            yield return null;
        }
        initialRot = transform.localRotation.eulerAngles;
        goalRot = initialRot;
        if (side < 0.0f)
            goalRot.z += 180.0f;
        else if (side > 0.0f)
            goalRot.z -= 180.0f;

        t = 0;
        while (t < barrelRollDuration / 2.0f)
        {
            currentRot.z = Mathf.Lerp(initialRot.z, goalRot.z, t / (barrelRollDuration / 2.0f));
            transform.localRotation = Quaternion.Euler(currentRot);
            t += Time.deltaTime;
            yield return null;
        }

        inBarrelRoll = false;
    }*/
}
