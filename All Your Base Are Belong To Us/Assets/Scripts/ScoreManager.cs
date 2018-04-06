using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public int score = 0;
    private static ScoreManager instance = null;
    public static ScoreManager Instance {
        get { return instance; }
    }

    private void Awake()
    {
        if(instance !=null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        gameObject.name = "$ScoreManager";
    }
    private void OnGUI()
    {
        GUILayout.Label("Score: " + score);
    }
}
