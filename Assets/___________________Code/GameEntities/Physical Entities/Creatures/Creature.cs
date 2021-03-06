using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// People
/// can attack and move
/// </summary>
public abstract class Creature : PhysicalEntity<StatsCharacter, SoundsCharacter, StateCharacter>
{
    protected ISlot[] slots;
    protected Slot[] limbs;
    public FactionAlignment corpseAlignment;
    public LayerMask corpseLayer => LayerMask.NameToLayer("Items");
    public abstract Vector2 LimbsDirection { get; }

    protected override void GetComponents()
    {
        base.GetComponents();
        slots = GetComponentsInChildren<ISlot>();
        limbs = GetComponentsInChildren<Slot>();
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
        if (state.CanAttack())
        {
            foreach (var limb in limbs)
            {
                limb.UseWeapon();
            }
        }
    }
    protected void DoAttack(int limbId)
    {
        if (state.CanAttack())
        {
            limbs[limbId].UseWeapon();
        }
    }

    protected void DoMove(int movement_H)
    {
        if (movement_H != 0)
        {
            var addVelocity = OnGround ?
                movement_H * stats.statsMovement.acceleration_H :
                movement_H * stats.statsMovement.acceleration_H * stats.statsMovement.speed_controll_inAir;
            Velocity_H += addVelocity;
        }
        Anim_SetBool("run", movement_H != 0 && OnGround);
    }
    protected void DoJump() => Velocity_V = stats.statsMovement.jumpSpeed;

    protected override void OnHit(Hit hit)
    {
        if (!state.IsDead)
            PlayAudio(stats.sounds.onHitScreams);
    }

    protected override void OnDeath(Hit hit)
    {
        state.alignment = corpseAlignment;
        gameObject.layer = corpseLayer;
        foreach (var slot in slots)
        {
            slot.OnHostDeath();
        }
    }

    public override void Flip_H(bool faceRight)
    {
        base.Flip_H(faceRight);
        state.isRight = faceRight;
    }

    protected Transform GetClosest(IEnumerable<Transform> transforms)
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;
        foreach (var tr in transforms)
        {
            var distance = Vector2.Distance(tr.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = tr;
            }
        }
        return closest;
    }
    protected Vector3 GetClosest(IEnumerable<Vector3> positions)
    {
        Vector3 closest = Vector2.zero;
        float closestDistance = float.MaxValue;
        foreach (var pos in positions)
        {
            var distance = Vector2.Distance(pos, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = pos;
            }
        }
        return closest;
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
    }
}
