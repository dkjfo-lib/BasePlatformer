using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PhysicalItem
{
    public ObjectType attackerType;
    public AttackStatsMelee attackStats;
    public bool isRight = true;
    [Space]
    public ParticleSystem addonParticleSystem;
    public ClipsCollection addonHitSound;

    protected override void Init()
    {
        base.Init();
        if (!isRight)
        {
            Flip_H();
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

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
        if (addonParticleSystem != null)
        {
            addonParticleSystem.transform.parent = transform.parent;
            Destroy(addonParticleSystem.gameObject, 5);
        }
        if (addonHitSound)
        {
            addonHitSound.PlayRandomClip();
        }
    }

    private void CastAttack()
    {
        attackStats.DoAttack(transform.position, isRight, attackerType);
    }

    protected override void AddOnDrawGizmos()
    {
        attackStats.OnGizmos(transform.position, isRight);
    }
}
