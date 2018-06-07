using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour {

    private Animator creditsAnim;
	// Use this for initialization
	void Start () {
        creditsAnim = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
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
        if (creditsAnim.GetCurrentAnimatorStateInfo(0).IsName("ended"))
            StartCoroutine("EndCredits");
    }

    private IEnumerator EndCredits()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.SetGameState(GameManager.StateType.MainMenu);
        GameManager.Instance.LoadScene(0);
    }
}
