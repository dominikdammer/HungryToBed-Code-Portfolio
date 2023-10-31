using System;
using Timers;
using UnityEngine;

public class Tower_SmallDispenser : Tower
{
    [Header("SubTower Variables")]
    [SerializeField,Tooltip("how often a tower can shoot, before idleing")]
    private int _targetUsagesAmount = 3;

    private float shootInterval = 2;
    private Action attack;
    Timer attackLoop;


    private void OnValidate()
    {
        //Only updates for new towers....
        if(shootInterval != tower.shootInterval)
        {
            tower.shootInterval = shootInterval;
        }
    }

    private void Start()
    {
        ///subtower holds SO and defines shootInterval
        ///but askes for SO value first
        shootInterval = tower.shootInterval;

        attack = Attack; 

        ///assigns a new timer to variable and calls it afterwards
        attackLoop = new Timer(this, shootInterval, (uint)_targetUsagesAmount - (uint)CurrentUsagesAmount, false, attack);
        TimersManager.SetTimer(attackLoop);
    }

    void Attack()
    {
        Attack(AttackType.DropAmmo, null, _targetUsagesAmount);
    }

    #region Generic Code
    /// <summary>
    /// Copy this to all towers
    /// </summary>

    private void Update()
    {
        if (CurrentUsagesAmount >= _targetUsagesAmount)
        { return; }

        ReevaluateAttack();

        HandleReloadBarTimer();
    }

    private void ReevaluateAttack()
    {
        if (attackLoop.ShouldClear && CurrentUsagesAmount != _targetUsagesAmount)
        {
            attackLoop = new Timer(this, shootInterval, (uint)_targetUsagesAmount - (uint)CurrentUsagesAmount, false, attack);
            Timers.TimersManager.SetTimer(attackLoop, true);
        }
    }
    void HandleReloadBarTimer()
    {
        if (attackLoop.CurrentLoopsCount == _targetUsagesAmount)
        {
            LastAttackTime = 0;
            reloadBar.Fill.fillAmount = 1;
        }
        else
        {
            reloadBar.UpdateBar(FireRate, attackLoop.CurrentCycleElapsedTime);
        }
    }
    #endregion
}
