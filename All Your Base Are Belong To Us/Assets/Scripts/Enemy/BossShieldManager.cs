using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldManager : ShieldManager {

    public GameObject winScreen;
    new void Start()
    {
        base.Start();
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.Instance.StopSoundEffects();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Bullet"))
                child.parent = null;
            else
                Destroy(child.gameObject);
        }
        StartCoroutine(BossDieAnimation());
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        PlayerHUDManager.Instance.SetEnemyShieldBarWidth(currentShield);
    }

    private IEnumerator BossDieAnimation()
    {
        for(int i = 0; i < 30; i++)
        {
            Vector3 explosionSpawn = Random.insideUnitSphere * 2;
            explosionSpawn.z = transform.position.z;
            Destroy(Instantiate(deathEffect, new Vector3(Random.Range(-6, 6), Random.Range(-6, 6), transform.position.z), Quaternion.identity), 1f);
            yield return new WaitForSeconds(0.2f);
        }
        LevelManager.Instance.LoopSectorActive(false);
        PlayerHUDManager.Instance.SetEnemyShieldBarActive(false);
        if (winScreen != null)
            winScreen.GetComponent<WinScreen>().Win();
        Destroy(gameObject);
    }
}
