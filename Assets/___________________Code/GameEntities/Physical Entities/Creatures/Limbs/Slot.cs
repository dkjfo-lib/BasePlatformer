using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class Slot : SeeAndHearEntity, ISlot, ICanHit
{
    public StatsWeapon weaponStats;

    public override bool isRight { get => Father.isRight; set => Father.isRight = value; }

    protected bool isAlive = true;

    public Creature Father { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public BoxCollider2D Collider { get; set; }
    public Transform FirePoint { get; set; }

    // ICanHit
    public Object CoreObject => Father;
    public bool IsSelfDamageOn => weaponStats.isSelfDamageOn.value;
    public bool IsFriendlyDamageOn => weaponStats.isFriendlyDamageOn.value;
    public bool IsEnemy(Faction faction) => Father.state.alignment.IsEnemy(faction);

    // SeeAndHearEntity
    public override Animator GetAnimator => GetComponentInChildren<Animator>();

    protected override void Init()
    {
        inited = true;
        if (weaponStats != null)
            Equip(weaponStats);
    }
    protected override void GetComponents()
    {
        base.GetComponents();
        Father = GetComponentInParent<Creature>();
        Collider = GetComponentInChildren<BoxCollider2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        FirePoint = transform.GetChild(0).GetChild(0);
    }

    private void Update()
    {
        if (weaponStats == null) return;

        Vector2 aimFrom = weaponStats.attack is AttackStatsRange ?
            isRight ? FirePoint.position - FirePoint.right * 10 : FirePoint.position + FirePoint.right * 10 :
            isRight ? transform.position - transform.right * 10 : transform.position + transform.right * 10;
        transform.right = isRight ?
            Father.LimbsDirection - aimFrom :
            aimFrom - Father.LimbsDirection;
    }

    public void Equip(StatsWeapon newWeaponStats)
    {
        weaponStats = newWeaponStats;
        Animator.runtimeAnimatorController = weaponStats.animator;
        // Melee Attack Collider Setup
        weaponStats.attack.SetUp(this);
    }

    public void UseWeapon()
    {
        if (!isAlive) return;
        if (Anim_ClipName("attack")) return;
        if (weaponStats.sounds.onAttackSounds != null)
        {
            PlayAudio(weaponStats.sounds.onAttackSounds);
        }
        // Range Attack
        weaponStats.attack.StartAttack(this);
        Anim_SetTrigger("attack");
    }

    // Melee Attack
    private void OnTriggerEnter2D(Collider2D collision)
    {
        weaponStats.attack.OnTrigger(this, collision);
    }

    public virtual void OnDeath()
    {
        SpriteRenderer.enabled = false;
    }
}

