using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : Creature
{
    public DetectGround wallDetector;
    public bool isFlying = false;

    // Weapon Stuff
    private Limb PreferedLimb => limbs.FirstOrDefault();
    private AttackStatsBase PreferedWeaponStats => PreferedLimb.equipedWeapon.stats.attack;
    private float PreferedWeaponCloseBorder => PreferedLimb.transform.localPosition.x + PreferedWeaponStats.closeBorder;
    private float PreferedWeaponFarBorder => PreferedLimb.transform.localPosition.x + PreferedWeaponStats.farBorder;

    // Enemy Aim Stuff
    public Transform enemySight;
    private List<Creature> enemiesInSight = new List<Creature>();
    private Vector3? target;
    private Vector3 DefaultTarget => isRight ? transform.position + 1000 * Vector3.right : transform.position - 1000 * Vector3.right;
    private Vector2 TargetVector => target.Value - transform.position;
    public override Vector2 LimbsDirection => target != null ?
        target.Value :
        DefaultTarget;

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (state.IsDead) return;

        HandleEnemySight();
        wallDetector.UpdateDetector(transform.position, isRight);

        Attack();
        bool isMoving = Move();
        Jump(isMoving);
    }

    void HandleEnemySight()
    {
        var enemiesCenters = enemiesInSight.Where(s =>
        {
            var vect = s.ObjectCenter - ObjectCenter;
            var hit = Physics2D.Raycast(ObjectCenter, vect, vect.magnitude, Layers.Ground);
            return hit.transform == null;
        }).Select(s => s.ObjectCenter);
        target = GetClosest(enemiesCenters);
    }

    bool Move()
    {
        int movement_H = 0;
        if (target != null)
        {
            // no weapon == run away
            if (PreferedLimb == null)
            {
                if (target.Value.x > transform.position.x)
                {
                    movement_H = -1;
                    if (isRight)
                        Flip_H(false);
                }
                if (target.Value.x < transform.position.x)
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
                if (target.Value.x > transform.position.x)
                {
                    if (targetsDistance > PreferedWeaponFarBorder)
                        movement_H = +1;
                    if (targetsDistance < PreferedWeaponCloseBorder)
                        movement_H = -1;
                    if (!isRight)
                        Flip_H(true);
                }
                if (target.Value.x < transform.position.x)
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
            if ((wallDetector.Detected && isMoving) || isFlying)
                DoJump();
    }
    void Attack()
    {
        if (target == null) return;
        if (PreferedLimb == null) return;
        float targetsDistance = TargetVector.magnitude;
        enemySight.right = isRight ? target.Value - enemySight.position : enemySight.position - target.Value;
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
        if (hit.isRight == state.isRight)
        {
            Flip_H(!hit.isRight);
        }
        // if not in battle
        if (target == null)
        {
            enemySight.right = hit.hitDirection;
        }
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        wallDetector.OnGizmos(transform.position, isRight);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var creature = collision.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            if (state.alignment.IsEnemy(creature.Faction))
            {
                enemiesInSight.Add(creature);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var creature = collision.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            enemiesInSight.Remove(creature);
        }
    }
}

