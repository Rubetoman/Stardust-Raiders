using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : BonusObjController
{
    public GameObject icon;

    protected override void OnTriggerEnter(Collider other)
    {
        icon.transform.parent = other.gameObject.transform;
        base.OnTriggerEnter(other);
        switch (GameManager.Instance.playerInfo.gunType)
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
    }
    protected override IEnumerator DestroyAnimation()
    {
        float t = 0.0f;
        var currentPosition = icon.transform.position;
        var currentScale = icon.transform.localScale;
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
