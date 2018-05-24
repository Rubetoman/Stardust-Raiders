using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldManager : ShieldManager {

    new void Start()
    {
        base.Start();
    }

    protected override void Die()
    {
        base.Die();
        LevelManager.Instance.LoopSectorActive(false);
        Destroy(gameObject);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        PlayerHUDManager.Instance.SetEnemyShieldBarWidth(currentShield);
    }
}
