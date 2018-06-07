using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class WinScreen : MonoBehaviour {

    public Image screenFader;         // Panel where all elements of the GameOver Screen are located
    public Text winText;
    public Text score;                  // Text tha will show the Total Score on the Game Over Screen

    private bool win = false;
    private float timer = 0;
    void Start () {
    }

    private void Update()
    {
        if (win)
        {
            if (Input.GetButtonDown("Start") && timer > 10f)
            {
                GameManager.Instance.LoadScene("credits");
            }
            timer += Time.unscaledDeltaTime;
        }
    }

    public void Win()
    {
        StartCoroutine("WinAnimation");
    }

    private IEnumerator WinAnimation()
    {
        AudioManager.Instance.StopSound();
        yield return new WaitForSeconds(10f);
        GameManager.Instance.SetGameState(GameManager.StateType.Credits);
        float t = 0.0f;
        win = true;
        screenFader.gameObject.SetActive(true);
        var screenColor = screenFader.color;
        var newScreenColor = screenColor;
        newScreenColor.a = 1f;
        while (t < 5)
        {
            t += Time.deltaTime;
            screenFader.color = Color.Lerp(screenColor, newScreenColor, t);
            yield return null;
        }
        winText.gameObject.SetActive(true);
        score.gameObject.SetActive(true);
        score.text = "Score: " + GameManager.Instance.GetTotalScore();
        Time.timeScale = 0f;
    }

}
