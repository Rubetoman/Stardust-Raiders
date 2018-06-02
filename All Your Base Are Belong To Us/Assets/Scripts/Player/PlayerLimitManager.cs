using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimitManager : MonoBehaviour {

    public GameObject player;           // Player GameObject
    public float showDistance = 0.2f;   // Distance from the limit needed to reach to show the arrows
    public float flickFrequency = 2.0f; // Time it will take to flick an arrow

    private bool vArrowAnimFree = true; // Variable that tells if the vertical arrow animation is free or being used
    private bool hArrowAnimFree = true; // Variable that tells if the horizontal arrow animation is free or being used
    private Vector2 limitBounds;

    // Use this for initialization
    void Start () {
        limitBounds = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
    }
	
	// Update is called once per frame
	void Update () {
        // Calculate the distance from the player to the center of the plane
        var distanceToCenterX = player.transform.localPosition.x - transform.localPosition.x;
        var distanceToCenterY = player.transform.localPosition.y - transform.localPosition.y;

        // Find if the player is near a plane limit
        if(distanceToCenterY / limitBounds.y > 1 - showDistance && vArrowAnimFree)
        {
            ShowArrows("up");
        }
        else if (distanceToCenterY / limitBounds.y < -1 + showDistance && vArrowAnimFree)
        {
            ShowArrows("down");
        }
        if (distanceToCenterX / limitBounds.x < -1 + showDistance && hArrowAnimFree)
        {
            ShowArrows("left");
        }
        else if (distanceToCenterX / limitBounds.x > 1 - showDistance && hArrowAnimFree)
        {
            ShowArrows("right");
        }
    }

    /// <summary>
    /// Calls to the Couroutine which plays the animation of the arrow specified.
    /// </summary>
    /// <param name="pos">Which limit arrow to show: up, down, left or right</param>
    private void ShowArrows(string pos)
    {
        switch (pos)
        {
            case "up":
                StartCoroutine("VerticalArrowAnimation", 0);
                break;
            case "down":
                StartCoroutine("VerticalArrowAnimation", 1);
                break;
            case "left":
                StartCoroutine("HorizontalArrowAnimation", 2);
                break;
            case "right":
                StartCoroutine("HorizontalArrowAnimation", 3);
                break;
        }
    }

    /// <summary>
    /// Makes the arrow indicated as parameter to flick with the indicated frequecy
    /// </summary>
    /// <param name="currentArrow">Number of the arrow to play the animation: 0->up, 1-> down, 2-> left, 3-> right</param>
    /// <returns></returns>
    private IEnumerator VerticalArrowAnimation(int currentArrow)
    {
        vArrowAnimFree = false;
        PlayerHUDManager.Instance.SetLimitArrowActive(currentArrow, true);
        yield return new WaitForSeconds(flickFrequency);
        PlayerHUDManager.Instance.SetLimitArrowActive(currentArrow, false);
        yield return new WaitForSeconds(flickFrequency);
        vArrowAnimFree = true;
    }

    private IEnumerator HorizontalArrowAnimation(int currentArrow)
    {
        hArrowAnimFree = false;
        PlayerHUDManager.Instance.SetLimitArrowActive(currentArrow, true);
        yield return new WaitForSeconds(flickFrequency);
        PlayerHUDManager.Instance.SetLimitArrowActive(currentArrow, false);
        yield return new WaitForSeconds(flickFrequency);
        hArrowAnimFree = true;
    }

    /// <summary>
    /// Function to change the limits of the plane
    /// </summary>
    /// <param name="newSize">Vector2 with the X and Y scale for the limitPlane</param>
    public void ChangeLimitPlaneScale(Vector2 newScale)
    {
        transform.localScale = newScale;
    }

    public string GetPlayerLocationInPlane(DivideType type)
    {
        // Calculate the distance from the player to the center of the plane
        var distanceToCenterX = player.transform.localPosition.x - transform.localPosition.x;
        var distanceToCenterY = player.transform.localPosition.y - transform.localPosition.y;
        switch (type)
        {
            case DivideType.Left_Right:
                if (distanceToCenterX < -1)
                    return "left";
                else if (distanceToCenterX > 1)
                    return "right";
                break;

            case DivideType.Up_Down:
                if (distanceToCenterY > 1)
                    return "up";
                else if (distanceToCenterY < -1)
                    return "down";
                break;
        }
        return "center";
    }
}
