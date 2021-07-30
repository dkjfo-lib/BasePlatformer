using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// People
/// can attack and move
/// </summary>
[RequireComponent(typeof(CharacterGUI))]
public class Creature : PhysicalItem<StatsCharacter, SoundsCharacter, StateCharacter>
{
    public AttackStatsBase attackStats;
    public Limb[] limbs;
    [HideInInspector] public Faction corpseFaction = Faction.Item;
    [HideInInspector] public int corpseLayer => Layers.Items;

    protected override void GetComponents()
    {
        base.GetComponents();
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    protected void DoAttack()
    {
        if (state.CanAttack(attackStats.cooldown))
        {
            state.timeLastAttack = Time.timeSinceLevelLoad;
            state.inAttack = true;
            Anim_SetTrigger("attack");
            StartCoroutine(WaitWhileAttack());
            PlaySound(stats.sounds.onAttackScreams);
        }
    }
    public void CastAttack()
    {
        attackStats.DoAttack(this, transform.position, state.isRight, stats.entityType);
    }
    IEnumerator WaitWhileAttack()
    {
        yield return WaitWhileAnim(stats.attackAnimationName);
        state.inAttack = false;
    }

    protected void DoMove(int movement_H)
    {
        if (movement_H != 0)
        {
            var addVelocity = OnGround ?
                movement_H * stats.statsMovement.acceleration_H :
                movement_H * stats.statsMovement.acceleration_H * stats.statsMovement.speed_controll_inAir;
            Velocity_H += addVelocity;
            if (movement_H < 0 && isRight || movement_H > 0 && !isRight)
            {
                Flip_H();
            }
        }
        Anim_SetBool("run", movement_H != 0 && OnGround);
    }
    protected void DoJump() => Velocity_V = stats.statsMovement.jumpSpeed;

    protected override void OnHit(Hit hit)
    {
        PlaySound(stats.sounds.onHitScreams);
    }
    protected override void OnDeath(Hit hit) { }

    public override void Flip_H()
    {
        base.Flip_H();
        state.isRight = !state.isRight;
    }

    protected Transform GetClosest(IEnumerable<Transform> transforms)
    {
        return transforms?.FirstOrDefault();
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        if (attackStats != null) attackStats.OnGizmos(transform.position, state.isRight);
    }
}
