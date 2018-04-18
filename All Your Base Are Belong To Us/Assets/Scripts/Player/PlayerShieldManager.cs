using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldManager : MonoBehaviour {
    public const int maxShield = 110;
    public int currentShield = maxShield;
    public float recoverTime = 2f;
    public RectTransform shieldBar;
    //public MeshRenderer[] meshes; 
    public MeshRenderer mesh;
    public Color hitColor;
    public Color recoverColor;
    public int flickCount = 5;
    public float flickRate = 0.1f;

    public bool damaged = false;

    private Color normalColor;

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
            Debug.Log("Dead!");
        }

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
