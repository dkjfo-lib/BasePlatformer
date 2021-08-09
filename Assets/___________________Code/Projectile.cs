using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// spawns from RangedAttack and performs it's attack upon OnGround signal
/// animations: idle die
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Projectile : PhysicalItem<StatsProjectile, SoundsWeapon, StateWeapon>
{
    public ObjectType attackerType;
    public override bool isRight => state.isRight;
    [Space]
    public ParticleSystem addonLiveParticles;
    public ClipsCollection addonHitSound;

    int GroundLayer;

    protected override void Init()
    {
        base.Init();
        GroundLayer = LayerMask.NameToLayer("Ground");
        Inertia += isRight ? (Vector2)transform.right * stats.acceleration_H : -(Vector2)transform.right * stats.acceleration_H;
        StartCoroutine(KillSelf(1));
    }

    protected override void OnFixedUpdate() { }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            KillSelf();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            KillSelf();
            return;
        }

        var target = collision.GetComponent<IHittable>();
        if (BaseExt.ShouldHit(this, target))
        {
            var returnForce = target.GetHit(new Hit
            {
                attackerType = stats.entityType,
                isRight = isRight,
                hitPosition = transform.position,
                hitDirection = transform.right,
                damage = (stats.attack as AttackStatsMelee).damage,
                force = (stats.attack as AttackStatsMelee).force
            });
            GetHit(new Hit
            {
                damage = (int)returnForce.magnitude,
                force = 0,
                attackerType = ObjectType.ITEM,
                hitPosition = transform.position,
                hitDirection = transform.right,
                isRight = isRight
            });
            StartCoroutine(KillSelf(stats.detonationDelay));
        }
    }

    IEnumerator KillSelf(float time)
    {
        yield return new WaitForSeconds(time);
        KillSelf();
    }

    void KillSelf()
    {
        GetHit(new Hit
        {
            damage = 99999,
            force = 0,
            attackerType = ObjectType.ITEM,
            hitPosition = transform.position,
            hitDirection = transform.right,
            isRight = isRight
        });
    }

    protected override void OnDeath(Hit hit)
    {
        base.OnDeath(hit);

        if (addonLiveParticles != null)
        {
            addonLiveParticles.transform.parent = transform.parent;
            Destroy(addonLiveParticles.gameObject, 2);
        }
        if (addonHitSound)
        {
            PlayAudio(addonHitSound);
        }
        Destroy(gameObject);
    }
}
