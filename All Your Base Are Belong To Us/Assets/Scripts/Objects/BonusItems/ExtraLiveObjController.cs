using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLiveObjController : BonusObjController
{
    public int extraLives = 1;
    public GameObject icon;

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerCollider"))
            return;
        icon.transform.parent = other.gameObject.transform;
        Destroy(icon, 2);
        base.OnTriggerEnter(other);
        GameManager.Instance.AddtPlayerLives(extraLives);
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
