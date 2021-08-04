using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class Limb : GraphicalItem, ISlot
{
    public WeaponDescription defaultWeapon;
    public WeaponDescription weapon;

    public Creature Father { get; protected set; }
    AudioSource AudioSource { get; set; }
    BoxCollider2D Collider { get; set; }
    SpriteRenderer SpriteRenderer { get; set; }
    Transform FirePoint { get; set; }

    public override bool isRight => Father.isRight;

    protected override void Init()
    {
        inited = true;
        Equip(weapon.stats != null ? weapon : defaultWeapon);
    }
    protected override void GetComponents()
    {
        Father = GetComponentInParent<Creature>();
        Animator = GetComponentInChildren<Animator>();
        AudioSource = GetComponentInChildren<AudioSource>();
        Collider = GetComponentInChildren<BoxCollider2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        FirePoint = transform.GetChild(0).GetChild(0);
    }

    private void Update()
    {
        Vector2 aim = weapon.stats.attack is AttackStatsRange ?
            FirePoint.position : transform.position;
        transform.right = isRight ?
            Father.LimbsDirection - aim :
            aim - Father.LimbsDirection;
    }

    public void Equip(WeaponDescription newWeapon)
    {
        weapon = newWeapon;
        weapon.state = newWeapon.state;
        Animator.runtimeAnimatorController = weapon.stats.animator;
        Collider.offset = weapon.stats.attack.offset;
        Collider.size = weapon.stats.attack.size;
    }

    public void UseWeapon()
    {
        if (Anim_ClipName("attack")) return;
        if (weapon.stats.sounds.onAttackSounds != null)
        {
            PlayAudio(weapon.stats.sounds.onAttackSounds);
        }
        /// Range Attack
        if (weapon.stats.attack is AttackStatsRange)
        {
            var newProjectile = Instantiate((weapon.stats.attack as AttackStatsRange).projectile, FirePoint.position, FirePoint.transform.rotation);
            newProjectile.state.isRight = Father.isRight;
            newProjectile.state.alignment = Father.state.alignment;
        }
        Anim_SetTrigger("attack");
    }

    /// <summary>
    /// Melee Attack
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.GetComponent<IHittable>();
        if (BaseExt.ShouldHit(this, target))
        {
            Vector2 returnForce = target.GetHit(new Hit
            {
                attackerType = Father.stats.entityType,
                isRight = Father.isRight,
                damage = (weapon.stats.attack as AttackStatsMelee).damage,
                force = (weapon.stats.attack as AttackStatsMelee).force,
            });
            Father.Inertia += returnForce;
        }
    }

    public void OnDeath()
    {
        SpriteRenderer.enabled = false;
    }
}

/// <summary>
/// Description of weapon and it's state
/// </summary>
[System.Serializable]
public class WeaponDescription
{
    public StatsWeapon stats;
    public StateWeapon state;
}

public interface IHittable
{
    Faction Faction { get; }
    Vector2 GetHit(Hit hit);
}

