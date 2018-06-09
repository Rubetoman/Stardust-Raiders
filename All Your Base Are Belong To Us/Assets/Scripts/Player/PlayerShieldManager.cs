using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShieldManager : ShieldManager {
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
        if(PlayerHUDManager.Instance != null)
            currentShield = PlayerHUDManager.Instance.GetPlayerShieldBarWidth(); // For changes between scenes set health by getting playerHUDManager value
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AudioManager.Instance.Play("Hurt");
        PlayerHUDManager.Instance.SetPlayerShieldBarWidth(currentShield);
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
        PlayerHUDManager.Instance.SetPlayerShieldBarWidth(currentShield);
    }

    protected override void Die()
    {
        base.Die();
        player.SetActive(false);
        LevelManager.Instance.PauseLevel();
        GameManager.Instance.playerInfo.isDead = true;
        if (GameManager.Instance.playerInfo.lives-1 >= 0)
        {
            Invoke("RespawnPlayer", 2.0f);
            LevelManager.Instance.Invoke("ContinueLevel", 2.5f);
        }
        else
        {
            //Game Over screen
            LevelManager.Instance.LevelGameOver();
        }
        GameManager.Instance.SubstractPlayerLives(1);
    } 

    void ResetPlayerShield()
    {
        currentShield = maxShield;
        PlayerHUDManager.Instance.SetPlayerShieldBarWidth(currentShield);
    }

    private void RespawnPlayer()
    {
        if (!GameManager.Instance.playerInfo.isDead) // Check if player is dead
            return;
        player.GetComponent<ShipController>().ResetBoost();                     // Reset boost bar and make available the boost/brake again
        player.GetComponent<BarrelRollController>().inBarrelRoll = false;       // Reset barrel roll state in case it died during barrel roll
        player.GetComponent<BarrelRollController>().ResetBarrelRollShield();    // Reset barrel roll shield effect
        player.SetActive(true);                                                 // Show player again
        ResetPlayerShield();                                                    // Reset Player Shield values
        player.SendMessageUpwards("ResetPosition");                             // Reset Player position on GameplayPlane
        player.SendMessageUpwards("ResetRotation");                             // Reset Player rotation on GameplayPlane
        invulnerable = true;                                                    // Make Player invulnerable untill flickering effect has dissapear
        StartCoroutine(FlickeringColor(recoverColor));                          // Play flickering effec
        GameManager.Instance.playerInfo.isDead = false;
    }
}
