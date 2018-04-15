using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public GameObject gameplayPlane;
    [Space(10)]
    [Header("Movement")]
    public Vector2 movementSpeed = Vector2.one;      // Speed at which the spaceship can move around the x and y axis
    [Range(-1, 1)]
    public int invertXAxis = 1;             // Invert horizontal movement (-1 for invert, else 1, with 0 axis disabled)
    [Range(-1, 1)]
    public int invertYAxis = 1;             // Invert vertical movement (-1 for invert, else 1, with 0 axis disabled)
    public float pointingDepth = 2.0f;      // Z axis distance to point the space towards
    public float maxRotDegrees = 230.0f;    // Max degrees of freedom for the rotation of the spaceship
    public float bankAmountOnTurn = 25.0f;  // How much will the ship bank when you mover horizontally
    [Space(10)]
    [Header("Boost & Brake")]
    public float boostDuration = 1.0f;      // Time it takes to do the boost, also time it takes to be back again at the start position
    public float recoilTime = 2.0f;         // Time it takes to the boos or brake to be ready again for re-use
    public int speedMod = 5;                // How much the speed will be increased or decreased while boosting or braking
    public const int maxBoost = 110;
    public RectTransform boostBar;

    private bool boostReady = true;


    // Use this for initialization
    void Start() {
        //var gamePlane = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //if(transform.position.x)
        ShipMovement(horizontal, vertical);

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
        Vector3 finalPosition = transform.localPosition;
        finalPosition.x += direction.x * movementSpeed.x * Time.deltaTime;
        finalPosition.y += direction.y * movementSpeed.y * Time.deltaTime;
        transform.localPosition = finalPosition;

        //Make the ship bank when moving
        var barrelRollScript = GetComponent<BarrelRollController>();
        if (!barrelRollScript.inBarrelRoll)
        {
            barrelRollScript.BankNoBarrelRoll("Horizontal", bankAmountOnTurn);
        }
        //Rotate towards the point which is moving
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(finalDirection), Mathf.Deg2Rad * maxRotDegrees);
    }

    private IEnumerator Boost()
    {
        boostReady = false;
        var t = 0f;
        var speedScript = gameplayPlane.GetComponent<RailMover>();
        var normalSpeed = speedScript.speed;
        movementSpeed.x *= 1.25f; //Make the ship to move faster horizontally   
        movementSpeed.y *= 1.25f; //Make the ship to move faster vertically
        //Go front in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.5f), t);
            speedScript.speed += speedMod;
            boostBar.sizeDelta = new Vector2(maxBoost * (1.0f - t), boostBar.sizeDelta.y);
            yield return null;
        }
        t = 0f;
        //Come back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.5f), t);
            //Decrease the speed and avoid to go under normal speed, also avoid to decrease it if the flow is not active
            if (speedScript.speed > normalSpeed)
                speedScript.speed -= speedMod;
            yield return null;
        }
        movementSpeed.x *= 0.8f; //Horizontal movement back to normal
        movementSpeed.y *= 0.8f; //Vertical movement back to normal
        speedScript.speed = normalSpeed; //Gameplay plane speed back to normal
        StartCoroutine("BoostBarRecoil", recoilTime);
    }

    private IEnumerator Brake()
    {
        boostReady = false;
        var t = 0f;
        var speedScript = gameplayPlane.GetComponent<RailMover>();
        var normalSpeed = speedScript.speed;
        movementSpeed.x *= 0.8f; //Slow horizontal movement
        movementSpeed.y *= 0.8f; //Slow vertical movement
        //Go back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.15f), t);
            //Decrease the speed and avoid to go under 0. Also avoid decreasing the speed if the flow is not active
            if(speedScript.speed > 0.0f)
                speedScript.speed -= speedMod;
            boostBar.sizeDelta = new Vector2(maxBoost * (1.0f - t), boostBar.sizeDelta.y);
            yield return null;
        }
        t = 0f;
        //Come back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.15f), t);
            //Increase back the speed and avoid to go over normal speed. Also avoid increasing the speed if the flow is not active
            if (speedScript.speed < normalSpeed)
                speedScript.speed += speedMod;
            yield return null;
        }
        movementSpeed.x *= 1.25f;   //Horizontal movement back to normal
        movementSpeed.y *= 1.25f;   //Vertical movement back to normal
        speedScript.speed = normalSpeed; //Gameplay plane speed back to normal
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
}
