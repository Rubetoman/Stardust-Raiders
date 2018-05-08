using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShieldManager : ShieldManager {
    public RectTransform playerShieldBar;
    [Header("Recover Effect")]
    public Color recoverColor;
    [HideInInspector]
    public bool inBarrelRoll = false;

    //private int currentLives;
    private GameObject player;
    new void Start()
    {
        base.Start();
        //currentLives = GameManager.Instance.playerInfo.lives;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        playerShieldBar.sizeDelta = new Vector2(currentShield, playerShieldBar.sizeDelta.y);
    }

    public void RecoverShield(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Negative numbers not allowed, if you want to low current shield you should use TakeDamage() instead");
            return;
        }
        var newShieldAmount = currentShield + amount;
        if (newShieldAmount + amount < maxShield)
            currentShield = newShieldAmount;
        else
            currentShield = maxShield;

        StartCoroutine(FlickeringColor(recoverColor));
        playerShieldBar.sizeDelta = new Vector2(currentShield, playerShieldBar.sizeDelta.y);
    }

    protected override void Die()
    {
        base.Die();
        player.SetActive(false);
        LevelManager.Instance.PauseLevel();
        GameManager.Instance.SubstractPlayerLives(1);

        if (GameManager.Instance.playerInfo.lives - 1 >= 0)
        {
            Invoke("RespawnPlayer", 2.0f);
            LevelManager.Instance.Invoke("ContinueLevel", 2.5f);
        }
        else
        {
            //Game Over screen
            print("Game Over");
            LevelManager.Instance.LevelGameOver();
        }
    } 

    private void RespawnPlayer()
    {
        player.GetComponent<ShipController>().ResetBoost();
        player.SetActive(true);
        currentShield = maxShield;
        playerShieldBar.sizeDelta = new Vector2(currentShield, playerShieldBar.sizeDelta.y);
        StartCoroutine(base.FlickeringColor(Color.blue));
        player.SendMessageUpwards("ResetPosition");
        player.SendMessageUpwards("ResetRotation");
    }
}
