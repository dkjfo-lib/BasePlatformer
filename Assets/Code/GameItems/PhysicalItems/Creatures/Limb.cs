using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class Limb : MonoBehaviour
{
    public WeaponDescription defaultWeapon;
    public WeaponDescription weapon;

    Creature Father { get; set; }
    Animator Animator { get; set; }
    AudioSource AudioSource { get; set; }
    BoxCollider2D Collider { get; set; }

    private void Start()
    {
        Father = GetComponentInParent<Creature>();
        Animator = GetComponentInChildren<Animator>();
        AudioSource = GetComponentInChildren<AudioSource>();
        Collider = GetComponentInChildren<BoxCollider2D>();
        Equip(weapon.stats != null ? weapon : defaultWeapon);
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
        PlaySound();
        StartCoroutine(KickStartAnimation());
    }
    void PlaySound()
    {
        AudioSource.Stop();
        AudioSource.clip = weapon.stats.sounds.onAttackSounds?.GetRandomClip();
        AudioSource.Play();
    }
    IEnumerator KickStartAnimation()
    {
        Animator.SetBool("attack", true);
        yield return new WaitForEndOfFrame();
        Animator.SetBool("attack", false);
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
        if (shoulHitOther) return alwaysHitted;
        return false;
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