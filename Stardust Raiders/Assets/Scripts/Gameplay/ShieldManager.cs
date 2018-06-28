using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script with basic functions to manage any shield. It lets to any GameObject to have a shild value, take damage and die.
/// The shield is what could be called the health of the GameObject.
/// </summary>
public class ShieldManager : MonoBehaviour {

    [Header("Shield")]
    public const int maxShield = 100;       // Maximun value for the shield, also the default value.
    public int currentShield = maxShield;   // Current value of the shield.
    public bool godMode = false;            // If true, this shield will not take any damage, but hurt animations will still play.
    [Space(10)]
    [Header("Hit Effect")]
    public float recoverTime = 2f;          // Time to be invulnerable after taking damage.
    public Color hitColor;                  // Color to flick when taking damage.
    public int flickCount = 5;              // Times the hit GameObject will flick between colors.
    public float flickRate = 0.1f;          // Time between color change.
    public GameObject deathEffect;          // GameObject to spawn after death.

    protected MeshRenderer mesh;            // MeshRenderer of the GameObject that contains the shield.
    protected Color normalColor;            // Default color of the GameObject without any effect is applied.

    [HideInInspector]
    public bool invulnerable = false;       // If true the shield will not take any damage, neither will play hurt animations.
    private bool flickeringColor = false;   // If true the flickering animation is playing (avoid to play more that one at a time).

    protected void Start () {
        mesh = gameObject.GetComponent<MeshRenderer>(); // Get MeshRenderer for flick color animation.
    }
	
    /// <summary>
    /// Function that manages the damage taken by the shield.
    /// </summary>
    /// <param name="amount"> Amount of damage taken.</param>
    public virtual void TakeDamage(int amount)
    {
        if (invulnerable)   // If invulnerable exit.
            return;
        if (amount < 0)
        {
            Debug.LogError("Negative numbers not allowed");
            return;
        }
        // Substract shield value if not in godMode.
        if (!godMode) {
            currentShield -= amount;
            if (currentShield <= 0)
            {
                currentShield = 0;
                Die();
            }
        }
        // Check if the gameObject containing the script is still active and has recover time.
        if (gameObject.activeInHierarchy && mesh != null && recoverTime > 0)    
            StartCoroutine(HitEffect());
    }

    /// <summary>
    /// Funtion to be called when shield reaches 0. It spawns deathEffect GameObject.
    /// </summary>
    protected virtual void Die()
    {
        Destroy(Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity), 1.0f);
    }

    /// <summary>
    /// Function that controlls the actions that will take place after the shield gets hit.
    /// </summary>
    protected IEnumerator HitEffect()
    {
        invulnerable = true;                            // Set the shield invulnerable untill the flickering animation has ended. 
        if(!flickeringColor)
            StartCoroutine(FlickeringColor(hitColor));  // Play the animation if it is not already being played.
        yield return null;
    }

    /// <summary>
    /// This Enumerator makes the ship flick the times specified by flickCount between the normal color and hitColor
    /// </summary>
    protected IEnumerator FlickeringColor(Color newColor)
    {
        flickeringColor = true;
        normalColor = mesh.material.color;
        for (int i = 0; i <= flickCount; i++)
        {
            mesh.material.color = newColor;             
            yield return new WaitForSeconds(flickRate);
            mesh.material.color = normalColor;
            yield return new WaitForSeconds(flickRate);
        }
        invulnerable = false;       //Be able to be damaged again
        flickeringColor = false;
    }
}
