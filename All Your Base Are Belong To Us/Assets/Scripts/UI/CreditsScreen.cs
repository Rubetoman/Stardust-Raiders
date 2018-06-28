using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the CreditsCanvas. It makes the credits text animation speed faster when Start is pressed.
/// </summary>
public class CreditsScreen : MonoBehaviour {

    private Animator creditsAnim;   // Animator for the credits Text animation.

	void Start () {
        // Get Animator from same gameObject.
        creditsAnim = gameObject.GetComponent<Animator>();
    }
	
	void Update () {
        // When "Start" is pressed toogle between faster speed or normal speed.
        if (Input.GetButtonDown("Start"))   
        {
            if (GameManager.Instance.GetGameState() == GameManager.StateType.Credits)
            {
                if(creditsAnim.speed < 2f)
                    creditsAnim.speed = 3f;
                else
                    creditsAnim.speed = 1f;
            }
        }
        // Check if the end of the animation has been reached.
        if (creditsAnim.GetCurrentAnimatorStateInfo(0).IsName("ended"))
            StartCoroutine("EndCredits");
    }

    /// <summary>
    /// At the end of the credits animation this function waits 5 seconds before loading the Main Menu.
    /// </summary>
    private IEnumerator EndCredits()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.LoadScene(0);
    }
}
