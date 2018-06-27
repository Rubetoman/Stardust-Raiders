using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the Bonus GameObject that upgrades Player's guns. Inherits from BonusObjController.
/// </summary>
public class PowerUpController : BonusObjController
{
    public GameObject icon;     // PowerUp icon. (Normally it is a child of the bonus GameObject). 

    protected override void OnTriggerEnter(Collider other)      // Only Player can pick it up.
    {
        if (!other.CompareTag("PlayerCollider"))
            return;
        icon.transform.parent = other.gameObject.transform;
        Destroy(icon, 2);
        base.OnTriggerEnter(other);
        switch (GameManager.Instance.playerInfo.gunType)        // Upgrade gun depending on the current.
        {
            default:
                GameManager.Instance.UpgradeGunType();
                break;
            case GunType.Single:
                GameManager.Instance.UpgradeGunType();
                break;
            case GunType.Dual:
                GameManager.Instance.UpgradeGunType();
                GameManager.Instance.AddToTotalScore(base.points/2);
                break;
            case GunType.Triple:
                GameManager.Instance.AddToTotalScore(base.points);
                break;
        }
        AudioManager.Instance.Play("PowerUp");                  // Play sound.
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
