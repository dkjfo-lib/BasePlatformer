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
    public Transform creatureSight;
    private List<Creature> creaturesInSight = new List<Creature>();
    public Vector3? target;
    private Vector3 DefaultTarget => isRight ? transform.position + creatureSight.right * 1000 : transform.position - creatureSight.right * 1000;
    private Vector2 TargetVector => target.Value - transform.position;
    public override Vector2 LimbsDirection =>
        target != null ?
            target.Value :
            lastEnemyPlace != null ?
                lastEnemyPlace.Value :
                DefaultTarget;

    // Enemy follow stuff
    public float investigateDistance = 3f;
    public Vector3? lastEnemyPlace;
    private Vector2 EnemyPlaceVector => lastEnemyPlace.Value - transform.position;

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
        var enemiesCenters = creaturesInSight.Select<Creature, Vector3?>(s =>
        {
            if (s.state.IsDead) return null;
            if (!state.alignment.IsEnemy(s.Faction)) return null;

            var vectC = s.ObjectCenter - ObjectCenter;
            var hitC = Physics2D.Raycast(ObjectCenter, vectC, vectC.magnitude, Layers.Ground);
            if (hitC.transform == null) return s.ObjectCenter;

            var vectT = s.ObjectTop - ObjectTop;
            var hitT = Physics2D.Raycast(ObjectTop, vectT, vectT.magnitude, Layers.Ground);
            if (hitT.transform == null) return s.ObjectTop;

            var vectB = s.ObjectBottom - ObjectBottom;
            var hitB = Physics2D.Raycast(ObjectBottom, vectB, vectB.magnitude, Layers.Ground);
            if (hitB.transform == null) return s.ObjectBottom;

            return null;
        }).Where(s => s.HasValue).Select(s => s.Value);
        target = GetClosest(enemiesCenters);

        if (target != null)
        {
            creatureSight.right = isRight ? target.Value - creatureSight.position : creatureSight.position - target.Value;
            lastEnemyPlace = target;
        }
        if (target == null && lastEnemyPlace != null)
        {
            creatureSight.right = isRight ? lastEnemyPlace.Value - creatureSight.position : creatureSight.position - lastEnemyPlace.Value;
        }
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

        if (target == null && lastEnemyPlace != null)
        {
            float targetsDistance = EnemyPlaceVector.magnitude;
            if (lastEnemyPlace.Value.x > transform.position.x)
            {
                if (targetsDistance > investigateDistance)
                {
                    movement_H = +1;
                }
                else
                {
                    lastEnemyPlace = null;
                }

                if (!isRight)
                    Flip_H(true);
            }
            if (lastEnemyPlace.Value.x < transform.position.x)
            {
                if (targetsDistance > investigateDistance)
                {
                    movement_H = -1;
                }
                else
                {
                    lastEnemyPlace = null;
                }

                if (isRight)
                    Flip_H(false);
            }
        }

        bool isMoving = movement_H != 0;
        DoMove(movement_H);
        return isMoving;
    }
    void Jump(bool isMoving)
    {
        if (OnGround)
        {
            if (isFlying ||
                (wallDetector.Detected && isMoving) ||
                (target != null && target.Value.y - transform.position.y > PreferedWeaponFarBorder) ||
                (lastEnemyPlace != null && lastEnemyPlace.Value.y - transform.position.y > PreferedWeaponFarBorder))
            {
                DoJump();
            }
        }
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


        if (hit.isRight == state.isRight)
        {
            Flip_H(!hit.isRight);
        }
        // if not in battle
        if (target == null)
        {
            creatureSight.right = hit.hitDirection;
            lastEnemyPlace = isRight ? hit.hitDirection * investigateDistance : -hit.hitDirection * investigateDistance;
        }
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        wallDetector.OnGizmos(transform.position, isRight);

        foreach (var s in creaturesInSight)
        {
            Gizmos.DrawLine(ObjectCenter, s.ObjectCenter);
            Gizmos.DrawLine(ObjectTop, s.ObjectTop);
            Gizmos.DrawLine(ObjectBottom, s.ObjectBottom);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var creature = collision.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            creaturesInSight.Add(creature);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var creature = collision.gameObject.GetComponent<Creature>();
        if (creature != null)
        {
            creaturesInSight.Remove(creature);
        }
    }
}

