using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class Limb : SeeAndHearEntity, ISlot
{
    public WeaponDescription defaultWeapon;
    public WeaponDescription equipedWeapon;

    public override bool isRight { get => Father.isRight; set => Father.isRight = value; }

    public Creature Father { get; set; }
    public BoxCollider2D Collider { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Transform FirePoint { get; set; }

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
        Vector2 aimFrom = equipedWeapon.stats.attack is AttackStatsRange ?
            isRight ? FirePoint.position - FirePoint.right * 10 : FirePoint.position + FirePoint.right * 10 :
            isRight ? transform.position - transform.right * 10 : transform.position + transform.right * 10;
        transform.right = isRight ?
            Father.LimbsDirection - aimFrom :
            aimFrom - Father.LimbsDirection;
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

