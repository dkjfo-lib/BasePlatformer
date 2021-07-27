using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : CharacterAbstract
{
    public string attackAnimName = "b_attack";
    [Space]
    public Faction[] enemyFactions;
    public DetectLayer enemyDetector;
    public DetectLayer wallDetector;

    protected bool EnemyDetected => enemyDetector.Detected;
    protected IEnumerable<CharacterAbstract> EnemiesInSight => enemyDetector.contacts.
                Select(s => s.gameObject.GetComponent<CharacterAbstract>()).
                Where(s => s != null).
                Where(s => enemyFactions.Contains(s.faction));

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        enemyDetector.UpdateDetector(transform.position, isRight);
        wallDetector.UpdateDetector(transform.position, isRight);
        Attack();
        bool isMoving = Move();
        Jump(isMoving);
    }

    bool Move()
    {
        if (!charState.CanMove) return false;
        int movement_H = 0;
        if (EnemyDetected)
        {
            var enemies = EnemiesInSight.Select(s => s.transform);
            var target = GetClosest(enemies);
            if (target != null)
            {
                if (target.position.x > transform.position.x && !charState.isRight)
                    Flip_H();
                if (target.position.x < transform.position.x && charState.isRight)
                    Flip_H();
                if (target.position.x > transform.position.x + attackStats.GetOffset(charState.isRight).x + attackStats.Size.x / 2)
                {
                    movement_H = +1;
                }
                if (target.position.x < transform.position.x + attackStats.GetOffset(charState.isRight).x - attackStats.Size.x / 2)
                {
                    movement_H = -1;
                }
            }
        }
        bool isMoving = movement_H != 0;
        if (isMoving)
        {
            var addVelocity = OnGround ?
                movement_H * physicalStats.Acceleration_H :
                movement_H * physicalStats.Acceleration_H * physicalStats.Speed_controll_inAir;
            AddVelocityH(addVelocity);
        }
        Anim_SetBool("run", isMoving && OnGround);
        return isMoving;
    }
    void Jump(bool isMoving)
    {
        if (OnGround)
            if (wallDetector.Detected && isMoving)
                SetVelocityV(physicalStats.JumpSpeed);
    }
    void Attack()
    {
        if (charState.IsDead) return;
        if (attackStats == null) return;
        if (attackStats.HasEnemies(transform.position, charState.isRight, enemyFactions).Length > 0)
        {
            DoAttack(attackAnimName);
        }
    }
    protected override void OnHit(Hit hit)
    {
        if (hit.isRight != charState.isRight)
        {
            Flip_H();
        }
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        enemyDetector.OnGizmos(transform.position, isRight);
        wallDetector.OnGizmos(transform.position, isRight);
    }
}

