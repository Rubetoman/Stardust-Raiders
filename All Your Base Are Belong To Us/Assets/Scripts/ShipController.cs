using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public GameObject gameplayPlane;
    [Space(10)]
    [Header("Movement")]
    public float movementSpeed = 1.0f; //Speed at which the spaceship can move around the x and y axis
    [Range(-1, 1)]
    public int invertXAxis = 1; //Invert horizontal movement (-1 for invert, else 1)
    [Range(-1, 1)]
    public int invertYAxis = 1; //Invert vertical movement (-1 for invert, else 1)
    public float pointingDepth = 2.0f; //Z axis distance to point the space towards
    [Space(10)]
    [Header("Bank & Barrell Roll")]
    public float maxRotDegrees = 230.0f; //Max degrees of freedom for the rotation of the spaceship
    public float multipleTapDelay = 1.0f; //Max delay between taps of a Input to be considered as consecutive taps
    public float barrelRollDuration = 1.0f; //Time it takes to do the barrel roll
    [Space(10)]
    [Header("Boost & Brake")]
    public float boostDuration = 1.0f; //Time it takes to do the boost, also time it takes to be back again at the start position
    public float recoilTime = 2.0f; //Time it takes to the boos or brake to be ready again for re-use
    public int speedMod = 5; //How much the speed will be increased or decreased while boosting or braking
    public const int maxBoost = 110;
    public RectTransform boostBar;

    private bool buttonDown = false;
    private bool inBarrelRoll = false;
    private bool boostReady = true;
    private float leftTimer = 1.0f;
    private float rightTimer = 1.0f;


    // Use this for initialization
    void Start() {
        //Start getting the choosen control type
        var gamePlane = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //if(transform.position.x)
        ShipMovement(horizontal, vertical);

        if (!inBarrelRoll)
        {
            float bankAxis = Input.GetAxis("Bank");
            BarrelRoll(bankAxis);
        }

        //BOOST
        if(Input.GetButtonDown("Boost") && boostReady)
        {
            StartCoroutine("Boost");
        }


        if (Input.GetButtonDown("Brake") && boostReady)
        {
            StartCoroutine("Brake");
        }
    }

    /// <summary>
    /// Function which controlls the vertical and horizontal movement of the ship. This function is called every update.
    /// The first inpunt refers to the horizontal movement updated by Input and the second one to the vertical
    /// </summary>
    private void ShipMovement(float h, float v)
    {
        //Input direction
        Vector3 direction = new Vector3(invertXAxis * h, invertYAxis * v, 0);
        //Pointing direction, taking in account Z axis
        Vector3 finalDirection = new Vector3(invertXAxis * h, invertYAxis * v, pointingDepth);
        //Position tranform by horizontal and vertical input
        transform.position += direction * movementSpeed * Time.deltaTime;
        //Rotate towards the point which is moving
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(finalDirection), Mathf.Deg2Rad * maxRotDegrees);
    }

    /// <summary>
    /// Function which controlls the barrel roll and banking of the ship. It is called every update while not already doing a barrel roll.
    /// The input is the value for the banking of the ship, updated by the Input. If the same banking direction is preshed quick enought the ship performs a barrel roll.
    /// </summary>
    private void BarrelRoll(float bank)
    {
        
        Vector3 newRotationEuler = transform.rotation.eulerAngles;
        newRotationEuler.z = -90 * bank;
        Quaternion newQuat = Quaternion.identity;
        newQuat.eulerAngles = newRotationEuler;
        transform.rotation = newQuat;

        if (bank == 0.0f)
        {
            buttonDown = false;
        }
        else if (buttonDown == false)
        {
            if (bank < 0.0f)
            {
                buttonDown = true;
                if (leftTimer < multipleTapDelay)
                {
                    StartCoroutine("BarrelRoll", -1);
                }
                leftTimer = 0.0f;
            }
            else if (bank > 0.0f)
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

    private IEnumerator Boost()
    {
        boostReady = false;
        var t = 0f;
        var speedScript = gameplayPlane.GetComponent<MoveForward>();
        var normalSpeed = speedScript.speed;
        movementSpeed += 10.0f; //Make the ship to move faster
        //Go front in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), t);
            speedScript.speed += speedMod;
            boostBar.sizeDelta = new Vector2(maxBoost * (1.0f - t), boostBar.sizeDelta.y);
            yield return null;
        }
        t = 0f;
        //Come back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), t);
            speedScript.speed -= speedMod;
            yield return null;
        }
        movementSpeed -= 10.0f;
        speedScript.speed = normalSpeed;
        StartCoroutine("BoostBarRecoil", recoilTime);
    }

    private IEnumerator Brake()
    {
        boostReady = false;
        var t = 0f;
        var speedScript = gameplayPlane.GetComponent<MoveForward>();
        var normalSpeed = speedScript.speed;
        //Go back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f), t);
            speedScript.speed -= speedMod;
            boostBar.sizeDelta = new Vector2(maxBoost * (1.0f - t), boostBar.sizeDelta.y);
            yield return null;
        }
        t = 0f;
        //Come back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f), t);
            speedScript.speed += speedMod;
            yield return null;
        }
        speedScript.speed = normalSpeed;
        StartCoroutine("BoostBarRecoil",recoilTime);
    }

    private IEnumerator BoostBarRecoil(float reTime)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / reTime;
            boostBar.sizeDelta = new Vector2(maxBoost * t, boostBar.sizeDelta.y);
            yield return null;
        }
        boostBar.sizeDelta = new Vector2(maxBoost, boostBar.sizeDelta.y);
        boostReady = true;
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
