using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : Creature
{
    public DetectHittable enemyDetector;
    public DetectGround wallDetector;

    public Limb PreferedLimb => limbs.FirstOrDefault();
    public AttackStatsBase PreferedWeaponStats => PreferedLimb.weapon.stats.attack;
    public float PreferedWeaponCloseBorder => PreferedLimb.transform.localPosition.x + PreferedWeaponStats.closeBorder;
    public float PreferedWeaponFarBorder => PreferedLimb.transform.localPosition.x + PreferedWeaponStats.farBorder;

    protected IEnumerable<Creature> EnemiesInSight => enemyDetector.Contacts.
        Where(s => state.alignment.IsEnemy(s.state.alignment.faction));
    public Transform target;
    public Vector2 DefaultPosition => isRight ? Vector3.right : -Vector3.right;
    public Vector2 TargetVector => target.position - transform.position;
    public override Vector2 LimbsDirection => target == null ? DefaultPosition : TargetVector;

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (state.IsDead) return;

        enemyDetector.UpdateDetector(transform.position, isRight);
        wallDetector.UpdateDetector(transform.position, isRight);
        Attack();
        bool isMoving = Move();
        Jump(isMoving);
    }

    bool Move()
    {
        int movement_H = 0;
        var enemiesTr = EnemiesInSight.Select(s => s.transform);
        target = GetClosest(enemiesTr);
        if (target != null)
        {
            // no weapon == run away
            if (PreferedLimb == null)
            {
                if (target.position.x > transform.position.x)
                {
                    movement_H = -1;
                    if (isRight)
                        Flip_H(false);
                }
                if (target.position.x < transform.position.x)
                {
                    movement_H = +1;
                    if (!isRight)
                        Flip_H(true);
                }
            }
            // with weapon == attack
            else
            {
                float targetsDistance = TargetVector.magnitude;
                if (target.position.x > transform.position.x)
                {
                    if (targetsDistance > PreferedWeaponFarBorder)
                        movement_H = +1;
                    if (targetsDistance < PreferedWeaponCloseBorder)
                        movement_H = -1;
                    if (!isRight)
                        Flip_H(true);
                }
                if (target.position.x < transform.position.x)
                {
                    if (targetsDistance > PreferedWeaponFarBorder)
                        movement_H = -1;
                    if (targetsDistance < PreferedWeaponCloseBorder)
                        movement_H = +1;
                    if (isRight)
                        Flip_H(false);
                }
            }
        }
        bool isMoving = movement_H != 0;
        DoMove(movement_H);
        return isMoving;
    }
    void Jump(bool isMoving)
    {
        if (OnGround)
            if (wallDetector.Detected && isMoving)
                DoJump();
    }
    void Attack()
    {
        if (target == null) return;
        if (PreferedLimb == null) return;
        float targetsDistance = TargetVector.magnitude;
        if (PreferedWeaponCloseBorder < targetsDistance && targetsDistance < PreferedWeaponFarBorder)
        {
            DoAttack();
        }
    }
    protected override void OnDeath(Hit hit)
    {
        base.OnDeath(hit);
        target = null;
    }
    protected override void OnHit(Hit hit)
    {
        base.OnHit(hit);
        if (hit.isRight != state.isRight)
        {
            Flip_H(!isRight);
        }
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        enemyDetector.OnGizmos(transform.position, isRight);
        wallDetector.OnGizmos(transform.position, isRight);
    }
}

