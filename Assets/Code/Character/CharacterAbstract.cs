using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbstract : PhysicalItem
{
    public CharStats charStats;
    public CharState charState;
    public Attack attack;

    public ClipsCollection hitSounds;
    public ClipsCollection attackScreams;
    public ClipsCollection hitScreams;
    public ClipsCollection deathScreams;

    protected override void OnStart()
    {
        charState.health = charStats.MaxHealth;
        attack.Init();
        GetComponents();
        inited = true;
    }

    protected override void OnFixedUpdate()
    {
        DampVelocity();
    }

    protected void DoAttack(string attackName)
    {
        if (charState.CanAttack && attack.CanAttack())
        {
            charState.inAttack = true;
            Anim_SetTrigger("attack");
            StartCoroutine(WaitWhileAttack(attackName));
        }
    }
    IEnumerator WaitWhileAttack(string attackName)
    {
        yield return WaitWhileAnim(attackName);
        charState.inAttack = false;
    }
    public void CastAttack() => attack.DoAttack(transform.position);
    public void GetHit(Hit hit)
    {
        charState.health -= hit.damage;
        if (hit.position.x < transform.position.x)
            AddVelocityH(hit.force);
        else
            AddVelocityH(-hit.force);
        if (charState.IsDead)
        {
            Anim_SetTrigger("die");
        }
        else
        {
            Anim_SetTrigger("hurt");
        }
    }

    public void PlayHitSound() => hitSounds.PlayRandomClip();
    public void PlayAttackScream() => attackScreams.PlayRandomClip();
    public void PlayHitScream() => hitScreams.PlayRandomClip();
    public void PlayDeathScream() => deathScreams.PlayRandomClip();
    public void PlaySound(ClipsCollection collection)
    {
        collection.PlayRandomClip();
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected override void Flip_H()
    {
        base.Flip_H();
        charState.isRight = !charState.isRight;
        if (attack != null) attack.Flip_H();
    }

    protected override void AddOnDrawGizmos()
    {
        Gizmos.color = Color.red;
        attack.OnGizmos(transform.position);
    }
}