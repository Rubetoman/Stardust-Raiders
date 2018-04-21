using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShieldManager : ShieldManager {
    public RectTransform playerShieldBar;
    [Header("Recover Effect")]
    public Color recoverColor;
    [Space(10)]
    [Header("Lives")]
    public int initialLives = 3;
    public Text livesCount;
    
    private int currentLives;
    private GameObject player;

    new void Start()
    {
        base.Start();
        currentLives = initialLives;
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
        if (currentLives-1 >= 0)
        {
            currentLives--;
            livesCount.text = "x" + currentLives;
            Invoke("RespawnPlayer", 2.0f);
        }
        else
        {
            //Game Over screen
            print("Game Over");
        }
    } 

    private void RespawnPlayer()
    {
        player.SetActive(true);
        currentShield = maxShield;
        playerShieldBar.sizeDelta = new Vector2(currentShield, playerShieldBar.sizeDelta.y);
        StartCoroutine(base.FlickeringColor(Color.white));
    }
}
