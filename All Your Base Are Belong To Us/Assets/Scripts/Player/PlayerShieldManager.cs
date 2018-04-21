using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShieldManager : MonoBehaviour {
    [Header("Shield")]
    public const int maxShield = 100;
    public int currentShield = maxShield;
    public RectTransform shieldBar;
    public bool damaged = false;
    [Space(10)]
    [Header("Hit Effect")]
    public float recoverTime = 2f;
    public Color hitColor;
    public Color recoverColor;
    public int flickCount = 5;
    public float flickRate = 0.1f;
    [Space(10)]
    [Header("Lives")]
    public int initialLives = 3;
    public Text livesCount;
    public GameObject playerDeathEffect;
    
    private int currentLives;
    private MeshRenderer mesh;
    private Color normalColor;
    private GameObject player;

    private void Start()
    {
        currentLives = initialLives;
        mesh = gameObject.GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void TakeDamage(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Negative numbers not allowed, if you want to increase current shield you should use RecoverShield() instead");
            return;
        }
        damaged = true;
        currentShield -= amount;
        if (currentShield <= 0)
        {
            currentShield = 0;
            Dead();
            Debug.Log("Dead!");
        }
        if(player.activeSelf)
            StartCoroutine(FlickeringColor(hitColor));
        shieldBar.sizeDelta = new Vector2(currentShield, shieldBar.sizeDelta.y);
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
        shieldBar.sizeDelta = new Vector2(currentShield, shieldBar.sizeDelta.y);
    }

    private void Dead()
    {
        Destroy(Instantiate(playerDeathEffect, player.transform.localPosition, Quaternion.identity), 1.0f);
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
        shieldBar.sizeDelta = new Vector2(currentShield, shieldBar.sizeDelta.y);
        StartCoroutine(FlickeringColor(Color.white));
    }
    /// <summary>
    /// This Enumerator makes the ship flick the times specified by flickCount between the normal color and hitColor
    /// </summary>
    IEnumerator FlickeringColor(Color newColor)
    {
        normalColor = mesh.material.color;
        for (int i=0; i<=flickCount; i++)
        {
            mesh.material.color = newColor;
            yield return new WaitForSeconds(flickRate);
            mesh.material.color = normalColor;
            yield return new WaitForSeconds(flickRate);
        }
        //Be able to be damaged again
        damaged = false;
    }
}
