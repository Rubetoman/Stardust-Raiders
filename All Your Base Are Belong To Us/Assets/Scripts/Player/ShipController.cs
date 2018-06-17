using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public GameObject gameplayPlane;        // Plane where all the player mechanics are performed
    public GameObject limitPlane;           // Plane which sets the movement window
    [Space(10)]
    [Header("Movement")]
    public Vector2 movementSpeed = Vector2.one; // Speed at which the spaceship can move around the x and y axis
    public float pointingDepth = 2.0f;          // Z axis distance to point the space towards
    public float maxRotDegrees = 230.0f;        // Max degrees of freedom for the rotation of the spaceship
    public float bankAmountOnTurn = 25.0f;      // How much will the ship bank when you mover horizontally
    public float rotationSpeed = 15f;           // Speed at which the ship will rotate to look at the point which the ship is pointing
    [Space(10)]
    [Header("Boost & Brake")]
    public float boostDuration = 1.0f;      // Time it takes to do the boost, also time it takes to be back again at the start position
    public float recoilTime = 2.0f;         // Time it takes to the boos or brake to be ready again for re-use
    public int speedMod = 5;                // How much the speed will be increased or decreased while boosting or braking
    public const int maxBoost = 100;

    private bool boostReady = true;
    private bool boostBlocked = false;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gameState == GameManager.StateType.Play)
        {
            // MOVEMENT
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (!GameManager.Instance.playerInfo.isDead)
                ShipMovement(horizontal, vertical);

            // BOOST
            if (Input.GetButtonDown("Boost") && boostReady && !boostBlocked)
            {
                StartCoroutine("Boost");
            }

            // BRAKE
            if (Input.GetButtonDown("Brake") && boostReady && !boostBlocked)
            {
                StartCoroutine("Brake");
            }
        }
    }

    /// <summary>
    /// Function which controlls the vertical and horizontal movement of the ship. This function is called every update.
    /// The first inpunt refers to the horizontal movement updated by Input and the second one to the vertical
    /// </summary>
    private void ShipMovement(float h, float v)
    {
        var invertXAxis = GameManager.Instance.playerInfo.invertXAxis;
        var invertYAxis = GameManager.Instance.playerInfo.invertYAxis;

        Vector3 direction = new Vector3((invertXAxis ? -1 : 1) * h, (invertYAxis ? -1 : 1) * v, 0);                     //Input direction
        Vector3 finalDirection = new Vector3((invertXAxis ? -1 : 1) * h, (invertYAxis ? -1 : 1) * v, pointingDepth);    //Pointing direction, taking in account Z axis

        //Position tranform by horizontal and vertical input
        Vector3 finalPosition = transform.localPosition;
        finalPosition.x += direction.x * movementSpeed.x * Time.deltaTime;
        finalPosition.y += direction.y * movementSpeed.y * Time.deltaTime;

        //Limit movement to the plane space
        finalPosition.x = Mathf.Clamp(finalPosition.x, -(limitPlane.transform.localScale.x / 2), (limitPlane.transform.localScale.x / 2));
        finalPosition.y = Mathf.Clamp(finalPosition.y, -(limitPlane.transform.localScale.y / 2), (limitPlane.transform.localScale.y / 2));
        transform.localPosition = finalPosition;

        //Rotate towards the point which is moving with the speed passed as parameter
        var newRot = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(finalDirection), Mathf.Deg2Rad * maxRotDegrees);
        newRot.x = Quaternion.Lerp(transform.localRotation, Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(finalDirection), Mathf.Deg2Rad * maxRotDegrees), Time.deltaTime * rotationSpeed).x;
        newRot.y = Quaternion.Lerp(transform.localRotation, Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(finalDirection), Mathf.Deg2Rad * maxRotDegrees), Time.deltaTime * rotationSpeed).y;
        
        //Make the ship bank when moving
        if (Input.GetAxis("Bank") == 0)   // Avoid banking when in barrelRoll or already banking
        {
            //Calculate the new Z rotation when ship is moving horizontally
            //maxBankAngle is divided by 10 because is in Euler Angles and we want Quaternion angles
            if (h != 0)
                newRot.z = Quaternion.RotateTowards(transform.localRotation, Quaternion.AngleAxis(h * (-180 / 10), Vector3.forward), Mathf.Deg2Rad * 180).z;
            else
                newRot.z = Quaternion.Lerp(transform.localRotation,newRot, Time.deltaTime * rotationSpeed).z;
        }
        transform.localRotation = newRot;    
    }

    void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    #region BoostandBrakFunctions
    public void ResetBoost()
    {
        PlayerHUDManager.Instance.SetBoostBarWidth(maxBoost);
        boostReady = true;
    }

    private IEnumerator Boost()
    {
        boostReady = false;
        var t = 0f;
        var speedScript = gameplayPlane.GetComponent<RailMover>();
        var normalSpeed = speedScript.speed;
        var startZ = transform.localPosition.z;
        AudioManager.Instance.Play("Boost");
        movementSpeed.x *= 1.25f; //Make the ship to move faster horizontally   
        movementSpeed.y *= 1.25f; //Make the ship to move faster vertically
        //Go front in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            if (Time.timeScale != 0)    // Avoid to execute this when the game was paused
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.5f), t);
                speedScript.speed += speedMod;
                PlayerHUDManager.Instance.SetBoostBarWidth(maxBoost * (1.0f - t));
            }
            yield return null;
        }
        t = 0f;
        //Come back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            if (Time.timeScale != 0)    // Avoid to execute this when the game was paused
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.5f), t);
                //Decrease the speed and avoid to go under normal speed
                if (speedScript.speed > normalSpeed)
                    speedScript.speed -= speedMod;
            }
            yield return null;
        }
        movementSpeed.x *= 0.8f; //Horizontal movement back to normal
        movementSpeed.y *= 0.8f; //Vertical movement back to normal
        speedScript.speed = normalSpeed; //Gameplay plane speed back to normal
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, startZ); //Reset Z position
        StartCoroutine("BoostBarRecoil", recoilTime);
    }

    private IEnumerator Brake()
    {
        boostReady = false;
        var t = 0f;
        var speedScript = gameplayPlane.GetComponent<RailMover>();
        var normalSpeed = speedScript.speed;
        var startZ = transform.localPosition.z;
        AudioManager.Instance.Play("Brake");
        movementSpeed.x *= 0.8f; //Slow horizontal movement
        movementSpeed.y *= 0.8f; //Slow vertical movement
        //Go back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            if (Time.timeScale != 0)    // Avoid to execute this when the game was paused
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.15f), t);
                //Decrease the speed and avoid to go under 0. Also avoid decreasing the speed if the flow is not active
                if (speedScript.speed > 0.0f)
                    speedScript.speed -= speedMod;
                PlayerHUDManager.Instance.SetBoostBarWidth(maxBoost * (1.0f - t));
            }
            yield return null;
        }
        t = 0f;
        //Come back in the time given by boostDuration
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            if (Time.timeScale != 0)    // Avoid to execute this when the game was paused
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.15f), t);
                //Increase back the speed and avoid to go over normal speed. Also avoid increasing the speed if the flow is not active
                if (speedScript.speed < normalSpeed)
                    speedScript.speed += speedMod;
            }
            yield return null;
        }
        movementSpeed.x *= 1.25f;   //Horizontal movement back to normal
        movementSpeed.y *= 1.25f;   //Vertical movement back to normal
        speedScript.speed = normalSpeed; //Gameplay plane speed back to normal
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, startZ); //Reset Z position
        StartCoroutine("BoostBarRecoil",recoilTime);
    }

    private IEnumerator BoostBarRecoil(float reTime)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / reTime;
            if (Time.timeScale != 0)    // Avoid to execute this when the game was paused
                PlayerHUDManager.Instance.SetBoostBarWidth(maxBoost * t);
            yield return null;
        }
        PlayerHUDManager.Instance.SetBoostBarWidth(maxBoost);
        boostReady = true;
    }

    public void BlockBoost(bool block)
    {
        boostBlocked = block;
    }
    #endregion
}
