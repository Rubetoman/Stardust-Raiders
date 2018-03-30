using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public float movementSpeed = 1.0f;
    public int invertXAxis = 1; //Invert horizontal movement (-1 for invert, else 1)
    public int invertYAxis = 1; //Invert vertical movement (-1 for invert, else 1)
    public float pointingDepth = 2.0f; //Z axis distance to point the space towards
    public float maxRotDegrees = 230.0f; //Max degrees of freedom for the rotation of the spaceship
    public float multipleTapDelay = 1.0f; //Max delay between taps of a Input to be considered as consecutive taps
    public float barrelRollDuration = 1.0f; //Time it takes to do the barrel roll

    private bool buttonDown = false;
    private bool inBarrelRoll = false;
    private float leftTimer = 1.0f;
    private float rightTimer = 1.0f;


    // Use this for initialization
    void Start() {
        //Start getting the choosen control type

    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Input direction
        Vector3 direction = new Vector3(invertXAxis * horizontal, invertYAxis * vertical, 0);
        //Pointing direction, taking in account Z axis
        Vector3 finalDirection = new Vector3(invertXAxis * horizontal, invertYAxis * vertical, pointingDepth);
        //Position tranform by horizontal and vertical input
        transform.position += direction * movementSpeed * Time.deltaTime;
        //Rotate towards the point which is moving
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(finalDirection), Mathf.Deg2Rad * maxRotDegrees);

        if (!inBarrelRoll)
        {
            float bankAxis = Input.GetAxis("Bank");
            Vector3 newRotationEuler = transform.rotation.eulerAngles;
            newRotationEuler.z = -90 * bankAxis;
            Quaternion newQuat = Quaternion.identity;
            newQuat.eulerAngles = newRotationEuler;
            transform.rotation = newQuat;

            if (bankAxis == 0.0f)
            {
                buttonDown = false;
            }
            else if (buttonDown == false)
            {
                if(bankAxis < 0.0f)
                {
                    buttonDown = true;
                    if (leftTimer < multipleTapDelay)
                    {
                        StartCoroutine("BarrelRoll", -1);
                    }
                    leftTimer = 0.0f;
                }
                else if(bankAxis > 0.0f)
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
    }
    /// <summary>
    /// When the function is called it makes the spaceship to do a barrelRoll to the side passed as parameter
    /// If side is -1 it means is rolling to the left, else it goes to the right
    /// </summary>
    private IEnumerator BarrelRoll(int side)
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
    }
}
