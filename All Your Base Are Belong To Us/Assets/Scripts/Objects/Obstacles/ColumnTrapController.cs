using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to controll column trap obstacle. Inherits from ObstacleController.
/// </summary>
public class ColumnTrapController : ObstacleController
{
    public GameObject[] columns;            // Columns 
    public float targetDistance = 100.0f;

    private GameObject gameplayPlane;
    private bool animationPlayed = false;
    private float offSet = 0;

	void Start () {
        // Set gameplayPlane variable.
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
	}
	
	void Update () {
        // Check if the Player is near enought.
        if (Vector3.Project(gameplayPlane.transform.position - transform.position, gameplayPlane.transform.forward).magnitude <= targetDistance && !animationPlayed)
        {
            foreach(GameObject column in columns)               // Play fall animation for each column row with a small offset.
            {
                StartCoroutine(FallAnimation(column, offSet));
                offSet += 0.5f;
            }
            animationPlayed = true;
        }
    }

    /// <summary>
    /// Animation for making a column row fall.
    /// </summary>
    /// <param name="column"> The column GameObject that contains the entire row.</param>
    /// <param name="waitTime"> Time to wait before starting the animation.</param>
    private IEnumerator FallAnimation(GameObject column, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        float t = 0.0f;
        column.SetActive(true);
        var targetPos = column.transform.localPosition;
        targetPos.z = 0.0f;
        // Make the column fall.
        while (t < 10f)
        {
            t += Time.deltaTime;
            column.transform.localPosition = Vector3.Lerp(column.transform.localPosition, targetPos, t/10f);
            yield return null;
        }
    }
}
