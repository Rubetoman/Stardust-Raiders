using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Script for the Win Canvas. Shows a wining screen onced the final boss has been defeated.
/// </summary>
public class WinScreen : MonoBehaviour {

    public Image screenFader;           // Panel where all elements of the GameOver Screen are located.
    public Text winText;                // Text with the wining message.
    public Text score;                  // Text that will show the Total Score on the Game Over Screen.

    private bool win = false;           // If the game has been won.
    private float timer = 0;            // Inside timer.
    void Start () {
    }

    private void Update()
    {
        if (win)
        {
            // Once the game was won and 10 seconds passed, if Start is pressed the Credits are show.
            if (Input.GetButtonDown("Start") && timer > 10f)    
            {
                GameManager.Instance.LoadScene("credits");
            }
            timer += Time.unscaledDeltaTime;    // Use unscaled because Delta Time is scaled to 0 to pause the game.
        }
    }

    /// <summary>
    /// Function to call the WinAnmation function.
    /// </summary>
    public void Win()
    {
        StartCoroutine("WinAnimation");
    }

    /// <summary>
    /// Animation for the Win Screen. 
    /// </summary>
    private IEnumerator WinAnimation()
    {
        AudioManager.Instance.StopEverySound();                             // Stop game sound.
        yield return new WaitForSeconds(10f);                               // Wait 10 seconds.
        GameManager.Instance.SetGameState(GameManager.StateType.Credits);   // Set game state to Credits.
        float t = 0.0f;                                                     
        win = true;
        screenFader.gameObject.SetActive(true);                             // Set active the win screen.
        var screenColor = screenFader.color;                                
        var newScreenColor = screenColor;
        newScreenColor.a = 1f;

        // Smoothly fade to the win screen.
        while (t < 5)
        {
            t += Time.deltaTime;
            screenFader.color = Color.Lerp(screenColor, newScreenColor, t);
            yield return null;
        }
        winText.gameObject.SetActive(true);                                 // Show win text.
        score.gameObject.SetActive(true);                                   // Show total score.
        score.text = "Score: " + GameManager.Instance.GetTotalScore();
        Time.timeScale = 0f;                                                // Pause time.
    }
}
