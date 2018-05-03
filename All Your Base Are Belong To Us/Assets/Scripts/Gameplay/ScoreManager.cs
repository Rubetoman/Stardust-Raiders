using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public int score = 0;

    #region SingletonAndAwake
    private static ScoreManager instance = null;
    public static ScoreManager Instance {
        get { return instance; }
    }

    private void Awake()
    {
        if(instance !=null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        gameObject.name = "$ScoreManager";
    }
    #endregion
    //Shows score on the screen
    /*private void OnGUI()
    {
        GUILayout.Label("Score: " + score);
    }*/

    private void OnDestroy()
    {
        GameManager.Instance.AddToTotalScore(score);
    }
}
