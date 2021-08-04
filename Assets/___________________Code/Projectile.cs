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
    public ParticleSystem addonOnDeathParticles;
    public ClipsCollection addonHitSound;


    Collider2D Collider2D { get; set; }
    protected override void GetComponents()
    {
        base.GetComponents();
        Collider2D = GetComponent<Collider2D>();
    }

    private bool detonating = false;
    protected override void Init()
    {
        base.Init();
        Destroy(gameObject, 1);
        Inertia += isRight ? (Vector2)transform.right * stats.acceleration_H : -(Vector2)transform.right * stats.acceleration_H;
    }

    private void Update()
    {
        if (OnGround)
        {
            if (!state.IsDead)
            GetHit(new Hit
            {
                damage = 99999,
                force = 0,
                attackerType = ObjectType.ITEM,
                hitPosition = transform.position,
                isRight = isRight
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.GetComponent<IHittable>();
        if (BaseExt.ShouldHit(this, target))
        {
            var returnForce = target.GetHit(new Hit
            {
                attackerType = stats.entityType,
                isRight = isRight,
                damage = (stats.attack as AttackStatsMelee).damage,
                force = (stats.attack as AttackStatsMelee).force
            });
            GetHit(new Hit
            {
                damage = (int)returnForce.magnitude,
                force = 0,
                attackerType = ObjectType.ITEM,
                hitPosition = transform.position,
                isRight = isRight
            });
            StartCoroutine(DelayedDetonation());
        }
    }

    IEnumerator DelayedDetonation()
    {
        yield return new WaitForSeconds(stats.detonationDelay);
        GetHit(new Hit
        {
            damage = 99999,
            force = 0,
            attackerType = ObjectType.ITEM,
            hitPosition = transform.position,
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
        if (addonOnDeathParticles != null)
        {
            var newParticle = Instantiate(addonOnDeathParticles, transform.position, addonOnDeathParticles.transform.rotation, transform.parent);
            Destroy(newParticle.gameObject, 2);
        }
        if (addonHitSound)
        {
            PlayAudio(addonHitSound);
        }
        Destroy(gameObject);
    }
}
