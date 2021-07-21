using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAbstract : PhysicalItem
{
    public CharStats charStats;
    public CharState charState;
    public AttackStatsBase attackStats;
    protected IAttackHandler attack;
    public ObjectType characterType = ObjectType.UNDEFINED;

    public ClipsCollection hitSounds;
    public ClipsCollection attackScreams;
    public ClipsCollection hitScreams;
    public ClipsCollection deathScreams;

    protected override void Init()
    {
        base.Init();
        attack = AttackHandlerHelper.GetAttackHandler(attackStats);
        charState.health = charStats.MaxHealth;
        if (!charState.isRight)
        {
            Flip_H();
            charState.isRight = !charState.isRight;
        }
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
    public void CastAttack() => attack.DoAttack(transform.position, charState.isRight, characterType);
    IEnumerator WaitWhileAttack(string attackName)
    {
        yield return WaitWhileAnim(attackName);
        charState.inAttack = false;
    }

    public void GetHit(Hit hit)
    {
        charState.health -= hit.damage;
        var force = hit.position.x < transform.position.x ?
            hit.force :
            -hit.force;
        AddVelocityH(force);
        if (charState.IsDead)
        {
            QuestController.OnEvent(new EventDescription
            {
                who = hit.attackerType,
                didWhat = EventType.kill,
                toWhom= characterType
            });
            Anim_SetTrigger("die");
        }
        else
        {
            Anim_SetTrigger("hurt");
            OnHit(hit);
        }
    }
    protected virtual void OnHit(Hit hit) { }

    public void PlayHitSound() => hitSounds.PlayRandomClip();
    public void PlayAttackScream() => attackScreams.PlayRandomClip();
    public void PlayHitScream() => hitScreams.PlayRandomClip();
    public void PlayDeathScream() => deathScreams.PlayRandomClip();
    public void PlaySound(ClipsCollection collection)
    {
        collection.PlayRandomClip();
    }

    public override void Flip_H()
    {
        base.Flip_H();
        charState.isRight = !charState.isRight;
    }

    protected Transform GetClosest(IEnumerable<Transform> transforms)
    {
        return transforms.First();
    }

    protected override void AddOnDrawGizmos()
    {
        if (attackStats != null) AttackHandlerHelper.OnGizmos(attackStats, transform.position, charState.isRight);
    }
}