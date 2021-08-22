using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot : Creature
{
    public DetectGround wallDetector;
    public bool isFlying = false;
    public Pipe_Events pipe_Events;

    // Weapon Stuff
    private Slot PreferedLimb;
    private AttackStatsBase PreferedWeaponStats => PreferedLimb.weaponStats.attack;
    private float PreferedWeaponCloseBorder => PreferedLimb.transform.localPosition.x + PreferedWeaponStats.closeBorder;
    private float PreferedWeaponFarBorder => PreferedLimb.transform.localPosition.x + PreferedWeaponStats.farBorder;

    // Enemy Aim Stuff
    public Transform creatureSight;
    private List<Creature> creaturesInSight = new List<Creature>();
    [Space]
    public bool seesTarget;
    public bool isLookingForTarget;
    public Vector3 lastEnemyPosition;
    public float investigatedDistance = 2f;
    public float OnHitDistance = 10f;

    private Vector3 DefaultTarget => isRight ? transform.position + creatureSight.right * 1000 : transform.position - creatureSight.right * 1000;
    public override Vector2 LimbsDirection =>
        seesTarget || isLookingForTarget ?
            lastEnemyPosition :
            DefaultTarget;

    protected override void Init()
    {
        base.Init();
        PreferedLimb = limbs.Where(s => s.weaponStats != null).OrderByDescending(s => s.weaponStats.attack.DPS).FirstOrDefault();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (state.IsDead) return;

        HandleEnemySight();
        wallDetector.UpdateDetector(transform.position, isRight);

        bool isMoving = Move();
        Attack();
        Jump(isMoving);
    }

    void HandleEnemySight()
    {
        bool sawTarget = seesTarget;
        var enemiesPositions = creaturesInSight.Select<Creature, Vector3?>(s =>
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
        }).Where(s => s.HasValue).Select(s => s.Value).ToArray();

        seesTarget = enemiesPositions.Length > 0;
        if (seesTarget)
        {
            pipe_Events.SendEvent((int)Faction + "EnemyDetected");
            lastEnemyPosition = GetClosest(enemiesPositions);
        }

        // OnTargetLost
        if (!seesTarget && sawTarget)
        {
            isLookingForTarget = true;
        }

        // Update sight
        if (seesTarget || isLookingForTarget)
        {
            creatureSight.right = isRight ? lastEnemyPosition - creatureSight.position : creatureSight.position - lastEnemyPosition;
        }
    }

    bool Move()
    {
        int movement_H = 0;
        if (seesTarget)
        {
            // no weapon == run away
            if (PreferedLimb == null)
            {
                if (lastEnemyPosition.x > transform.position.x)
                {
                    movement_H = -1;
                    if (isRight)
                        Flip_H(false);
                }
                if (lastEnemyPosition.x < transform.position.x)
                {
                    movement_H = +1;
                    if (!isRight)
                        Flip_H(true);
                }
            }
            // with weapon == attack
            else
            {
                float targetsDistance = (lastEnemyPosition - transform.position).magnitude;
                if (lastEnemyPosition.x > transform.position.x)
                {
                    if (targetsDistance > PreferedWeaponFarBorder)
                        movement_H = +1;
                    if (targetsDistance < PreferedWeaponCloseBorder)
                        movement_H = -1;
                    if (!isRight)
                        Flip_H(true);
                }
                if (lastEnemyPosition.x < transform.position.x)
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

        if (!seesTarget && isLookingForTarget)
        {
            float targetsDistance = (lastEnemyPosition - transform.position).magnitude;
            if (lastEnemyPosition.x > transform.position.x)
            {
                if (targetsDistance > investigatedDistance)
                {
                    movement_H = +1;
                }
                else
                {
                    isLookingForTarget = false;
                }

                if (!isRight)
                    Flip_H(true);
            }
            else if (lastEnemyPosition.x < transform.position.x)
            {
                if (targetsDistance > investigatedDistance)
                {
                    movement_H = -1;
                }
                else
                {
                    isLookingForTarget = false;
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
            float distanceToJump = PreferedLimb != null ? PreferedWeaponFarBorder : investigatedDistance;
            if (isFlying)
            {
                DoJump();
            }
            else if (wallDetector.Detected && isMoving)
            {
                DoJump();
            }
            else if ((seesTarget || isLookingForTarget) && lastEnemyPosition.y - transform.position.y > distanceToJump)
            {
                DoJump();
            }
        }
    }
    void Attack()
    {
        if (!seesTarget) return;
        if (PreferedLimb == null) return;
        float targetsDistance = (lastEnemyPosition - transform.position).magnitude;
        if (PreferedWeaponCloseBorder < targetsDistance && targetsDistance < PreferedWeaponFarBorder)
        {
            DoAttack();
        }
    }
    protected override void OnDeath(Hit hit)
    {
        base.OnDeath(hit);
        seesTarget = false;
        isLookingForTarget = false;
    }
    protected override void OnHit(Hit hit)
    {
        base.OnHit(hit);
        if (hit.isRight == isRight)
        {
            Flip_H(!isRight);
        }
        // if not in battle
        if (!seesTarget)
        {
            isLookingForTarget = true;
            lastEnemyPosition = (Vector2)transform.position - hit.GetHitDirection() * OnHitDistance;
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

