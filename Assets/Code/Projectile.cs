using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PhysicalItem
{
    public ObjectType attackerType;
    public AttackStatsMelee attackStats;
    public bool facesRight = true;
    public override bool isRight => facesRight;
    [Space]
    public ParticleSystem addonParticleSystem;
    public ClipsCollection addonHitSound;

    protected override void Init()
    {
        base.Init();
        if (!facesRight)
        {
            Flip_H();
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        float movement_H = facesRight ? 1 : -1;
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
        attackStats.DoAttack(null, transform.position, facesRight, attackerType);
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        attackStats.OnGizmos(transform.position, facesRight);
    }
}
