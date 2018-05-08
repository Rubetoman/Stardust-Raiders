using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnTrapController : ObstacleController
{
    public GameObject[] columns;
    public float targetDistance = 100.0f;

    private GameObject gameplayPlane;
    private bool animationPlayed = false;
    private float offSet = 0;
	// Use this for initialization
	void Start () {
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Project(gameplayPlane.transform.position - transform.position, gameplayPlane.transform.forward).magnitude <= targetDistance && !animationPlayed)
        {
            foreach(GameObject column in columns)
            {
                StartCoroutine(FallAnimation(column, offSet));
                offSet += 0.5f;
            }
            animationPlayed = true;
        }

    }

    private IEnumerator FallAnimation(GameObject column, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        float t = 0.0f;
        column.SetActive(true);
        var targetPos = column.transform.localPosition;
        targetPos.z = 0.0f;

        while (t < 10f)
        {
            t += Time.deltaTime;
            column.transform.localPosition = Vector3.Lerp(column.transform.localPosition, targetPos, t/10f);
            yield return null;
        }

    }
}
