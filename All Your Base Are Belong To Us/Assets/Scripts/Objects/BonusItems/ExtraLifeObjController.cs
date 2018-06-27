using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the Bonus GameObject that gives an extra life to Player. Inherits from BonusObjController.
/// </summary>
public class ExtraLifeObjController : BonusObjController
{
    public int extraLives = 1;  // Number of extra lives given to player.
    public GameObject icon;     // ExtraLife icon. (Normally it is a child of the bonus GameObject).

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerCollider"))            // Only Player can pick it up.
            return;
        icon.transform.parent = other.gameObject.transform; // Make the icon child of Player.
        Destroy(icon, 2);
        base.OnTriggerEnter(other);
        GameManager.Instance.AddPlayerLives(extraLives);    // Add lives to Player.
    }

    /// <summary>
    /// Overriden unction that controlls the animation for the Bonus GameObject onced it is picked.
    /// Icon is also moved.
    /// </summary>
    protected override IEnumerator DestroyAnimation()
    {
        float t = 0.0f;
        var currentPosition = icon.transform.position;
        var currentScale = icon.transform.localScale;
        // Move smoothly towards Player. 
        while (t < 3)
        {
            t += Time.deltaTime;
            icon.transform.position = Vector3.Lerp(currentPosition, transform.parent.position, t / 0.5f);
            transform.position = icon.transform.position;
            icon.transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, t / 1f);
            transform.localScale = icon.transform.localScale;
            yield return null;
        }
    }
}
