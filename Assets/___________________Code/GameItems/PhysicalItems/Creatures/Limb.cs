using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class Limb : GraphicalItem, ISlot
{
    public WeaponDescription defaultWeapon;
    public WeaponDescription equipedWeapon;

    public override bool isRight => Father.isRight;

    public Creature Father { get; protected set; }
    public BoxCollider2D Collider { get; protected set; }
    SpriteRenderer SpriteRenderer { get; set; }
    public Transform FirePoint { get; protected set; }

    protected override void Init()
    {
        inited = true;
        Equip(equipedWeapon.stats != null ? equipedWeapon : defaultWeapon);
    }
    protected override void GetComponents()
    {
        Father = GetComponentInParent<Creature>();
        Animator = GetComponentInChildren<Animator>();
        Collider = GetComponentInChildren<BoxCollider2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        FirePoint = transform.GetChild(0).GetChild(0);
    }

    private void Update()
    {
        Vector2 aim = equipedWeapon.stats.attack is AttackStatsRange ?
            FirePoint.position : transform.position;
        transform.right = isRight ?
            Father.LimbsDirection - aim :
            aim - Father.LimbsDirection;
    }

    public void Equip(WeaponDescription newWeapon)
    {
        equipedWeapon = newWeapon;
        equipedWeapon.state = newWeapon.state;
        Animator.runtimeAnimatorController = equipedWeapon.stats.animator;
        // Melee Attack Collider Setup
        equipedWeapon.stats.attack.SetUp(this);
    }

    public void UseWeapon()
    {
        if (Anim_ClipName("attack")) return;
        if (equipedWeapon.stats.sounds.onAttackSounds != null)
        {
            PlayAudio(equipedWeapon.stats.sounds.onAttackSounds);
        }
        // Range Attack
        equipedWeapon.stats.attack.StartAttack(this);
        Anim_SetTrigger("attack");
    }

    // Melee Attack
    private void OnTriggerEnter2D(Collider2D collision)
    {
        equipedWeapon.stats.attack.OnTrigger(this, collision);
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

