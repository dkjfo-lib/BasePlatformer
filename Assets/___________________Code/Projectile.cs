using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// spawns from RangedAttack and performs it's attack upon OnGround signal
/// animations: idle die
/// </summary>
public class Projectile : PhysicalItem<StatsProjectile, SoundsWeapon, StateWeapon>
{
    public ObjectType attackerType;
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
            Flip_H(!facesRight);
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        float movement_H = facesRight ? 1 : -1;
        Velocity_H = movement_H * stats.acceleration_H;

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
            var newParticle = Instantiate(addonParticleSystem, transform.position, addonParticleSystem.transform.rotation, transform.parent);
            Destroy(newParticle.gameObject, 5);
        }
        if (addonHitSound)
        {
            addonHitSound.PlayRandomClip();
        }
    }

    private void CastAttack()
    {
        // TODO animations
        //stats.attack.DoAttack(null, transform.position, facesRight, attackerType);
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        // TODO colliders
        //stats.attack.OnGizmos(transform.position, facesRight);
    }
}
