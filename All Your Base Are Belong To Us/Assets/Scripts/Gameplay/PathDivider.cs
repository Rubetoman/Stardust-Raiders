using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DivideType
{
    Up_Down,
    Left_Right,
}
public class PathDivider : MonoBehaviour {


    public DivideType divideType;
    //public Rail railA;
 
    public GameObject[] arrows;         // The UI arrows which appear pointing both paths. Must be inserted in the following order: (up, low, left, right).
    public GameObject text;
    public float chooseTime = 5.0f;
    public float flickFrequency = 2.0f; // Time it will take to flick an arrow

    public GameObject limitPlane;
    private GameObject gameplayPlane;
    private bool pathSelection = false;
    private Rail altRail;
    private bool arrowAnimFree = true;
    private float timer = 0.0f;
    // Use this for initialization
    void Start () {
        gameplayPlane = GameObject.FindGameObjectWithTag("GameplayPlane");
    }
	
	// Update is called once per frame
	void Update () {
        if (pathSelection)
        {
            ChoosePath(altRail);
        }
	}

    public void ChoosePath(Rail newRail) {
        timer += Time.deltaTime;
        var position = limitPlane.GetComponent<PlayerLimitManager>().PlayerLocationInPlane();
        switch (divideType)
        {
            case DivideType.Up_Down:
                if (position == "up")
                {
                    //Animation of arrows
                    if(arrowAnimFree)
                        StartCoroutine("ArrowAnimation", 0);
                    //stop showing arrows
                    if (timer > chooseTime)
                    {
                        timer = 0.0f;
                        pathSelection = false;
                    }
                }
                else
                {
                    //Animation of arrows
                    if (arrowAnimFree)
                        StartCoroutine("ArrowAnimation", 1);
                    //change rail
                    if (timer > chooseTime)
                    {
                        ChangeRail();
                        timer = 0.0f;
                        pathSelection = false;
                    }
                }
                break;
            case DivideType.Left_Right:
                if (position == "left")
                {
                    //Animation of arrows
                    if (arrowAnimFree)
                        StartCoroutine("ArrowAnimation", 2);
                    //stop showing arrows
                    if (timer > chooseTime)
                    {
                        timer = 0.0f;
                        pathSelection = false;
                    }
                }
                else
                {
                    //Animation of arrows
                    if (arrowAnimFree)
                        StartCoroutine("ArrowAnimation", 3);
                    //change rail
                    if (timer > chooseTime)
                    {
                        ChangeRail();
                        timer = 0.0f;
                        pathSelection = false;
                    }

                }
                break;
        }
    }

    private void ChangeRail()
    {
        gameplayPlane.GetComponent<RailMover>().rail = altRail;
        print("changing rail");
    }

    private IEnumerator ArrowAnimation(int currentArrow)
    {
        arrowAnimFree = false;
        arrows[currentArrow].gameObject.SetActive(true);
        yield return new WaitForSeconds(flickFrequency);
        arrows[currentArrow].gameObject.SetActive(false);
        yield return new WaitForSeconds(flickFrequency);
        arrowAnimFree = true;
    }

    private IEnumerator TextAnimation()
    {
        while (timer < 2.0f)
        {
            text.gameObject.SetActive(true);
            yield return new WaitForSeconds(flickFrequency);
            text.gameObject.SetActive(false);
            yield return new WaitForSeconds(flickFrequency);
        }
    }

    public void activatePathSelection(Rail r)
    {
        altRail = r;
        pathSelection = true;
        StartCoroutine("TextAnimation");
    }
}
