using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class Limb : GraphicalItem
{
    public WeaponDescription defaultWeapon;
    public WeaponDescription weapon;

    Creature Father { get; set; }
    AudioSource AudioSource { get; set; }
    BoxCollider2D Collider { get; set; }

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
    }

    private void Update()
    {
        transform.up = Father.LimbsDirection;
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
            PlaySound(AudioSource, weapon.stats.sounds.onAttackSounds.GetRandomClip());
        }
        StartCoroutine(KickStartAnimation());
    }
    IEnumerator KickStartAnimation()
    {
        Anim_SetBool("attack", true);
        yield return new WaitForEndOfFrame();
        Anim_SetBool("attack", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.GetComponent<IHittable>();
        if (ShouldHit(target))
        {
            float returnForce = target.GetHit(new Hit
            {
                attackerType = Father.stats.entityType,
                isRight = Father.isRight,
                damage = (weapon.stats.attack as AttackStatsMelee).damage,
                force = (weapon.stats.attack as AttackStatsMelee).force,
            });
            Father.Inertia_H += Father.isRight ? -returnForce : returnForce;
        }
    }

    bool ShouldHit(IHittable hittable)
    {
        if (hittable == null) return false;

        bool isOneself = Father == (Object)hittable;
        bool selfDamageIsOn = weapon.stats.isSelfDamageOn.value;
        bool shoulHitSelf = isOneself && selfDamageIsOn;
        if (shoulHitSelf) return true;

        bool friendlyFireIsOn = weapon.stats.isFriendlyDamageOn.value;
        bool isEnemy = Father.state.alignment.IsEnemy(hittable.Faction);
        bool shoulHitOther = !isOneself && (friendlyFireIsOn || isEnemy);
        if (shoulHitOther) return true;

        bool alwaysHitted = hittable.Faction == Faction.Item_AllDamage;
        if (alwaysHitted) return alwaysHitted;
        return false;
    }

    public void DropWeapon()
    {

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
    float GetHit(Hit hit);
}