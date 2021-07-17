using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PhysicalItem
{
    public AttackStatsMelee attackStats;
    private AttackHandlerMelee attack;
    public bool isRight = true;

    protected override void Init()
    {
        base.Init();
        attack = new AttackHandlerMelee(attackStats);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (!isRight)
            Flip_H();

        float movement_H = isRight ? 1 : -1;
        SetVelocityH(movement_H * physicalStats.Acceleration_H);

        if (OnGround)
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        Anim_SetTrigger("die");
    }

    private void CastAttack()
    {
        attack.DoAttack(transform.position, isRight);
    }

    protected override void AddOnDrawGizmos()
    {
        AttackHandlerHelper.OnGizmos(attackStats, transform.position, isRight);
    }
}
