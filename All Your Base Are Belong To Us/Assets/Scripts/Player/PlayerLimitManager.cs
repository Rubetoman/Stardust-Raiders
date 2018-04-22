﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimitManager : MonoBehaviour {

    public GameObject player;           // Player GameObject
    public GameObject[] arrows;         // The UI arrows which appear when you reach the limit. Must be inserted in the following order: up, low, left, right.
    public float showDistance = 0.2f;   // Distance from the limit needed to reach to show the arrows
    public float flickFrequency = 2.0f; // Time it will take to flick an arrow

    private Bounds limitBounds;         // The bounds of limitPlane
    private bool vArrowAnimFree = true; // Variable that tells if the vertical arrow animation is free or being used
    private bool hArrowAnimFree = true; // Variable that tells if the horizontal arrow animation is free or being used

    // Use this for initialization
    void Start () {
        if(arrows.Length < 4)
        {
            Debug.LogError("The are arrows missing. Make sure to insert the 4 arrows in the following order: upper arrow, lower arrow, left arrow, right arrow");
            return;
        }
        limitBounds = GetComponent<Renderer>().bounds;
    }
	
	// Update is called once per frame
	void Update () {
        // Calculate the distance to the center
        var distanceToCenterX = player.transform.localPosition.x - limitBounds.center.x;
        var distanceToCenterY = player.transform.localPosition.y - limitBounds.center.y;

        // Find near which limit is the player
        if(distanceToCenterY / limitBounds.max.y > 1 - showDistance && vArrowAnimFree)
        {
            ShowArrows("up");
        }
        else if (distanceToCenterY / limitBounds.max.y < -1 + showDistance && vArrowAnimFree)
        {
            ShowArrows("down");
        }
        if (distanceToCenterX / limitBounds.max.x < -1 + showDistance && hArrowAnimFree)
        {
            ShowArrows("left");
        }
        else if (distanceToCenterX / limitBounds.max.x > 1 - showDistance && hArrowAnimFree)
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
        arrows[currentArrow].gameObject.SetActive(true);
        yield return new WaitForSeconds(flickFrequency);
        arrows[currentArrow].gameObject.SetActive(false);
        yield return new WaitForSeconds(flickFrequency);
        vArrowAnimFree = true;
    }

    private IEnumerator HorizontalArrowAnimation(int currentArrow)
    {
        hArrowAnimFree = false;
        arrows[currentArrow].gameObject.SetActive(true);
        yield return new WaitForSeconds(flickFrequency);
        arrows[currentArrow].gameObject.SetActive(false);
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

}