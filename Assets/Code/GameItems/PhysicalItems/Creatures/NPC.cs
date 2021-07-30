using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : Creature
{
    public DetectHittable enemyDetector;
    public DetectGround wallDetector;

    protected IEnumerable<Creature> EnemiesInSight => enemyDetector.Contacts.
        Where(s => state.alignment.enemyFactions.Contains(s.state.alignment.faction));

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
        if (!state.CanMove) return false;
        int movement_H = 0;
        var enemiesTr = EnemiesInSight.Select(s => s.transform);
        var target = GetClosest(enemiesTr);
        if (target != null)
        {
            if (target.position.x > transform.position.x && !state.isRight)
                Flip_H();
            if (target.position.x < transform.position.x && state.isRight)
                Flip_H();
            if (target.position.x > transform.position.x + attackStats.GetOffset(state.isRight).x + attackStats.Size.x / 2)
            {
                movement_H = +1;
            }
            if (target.position.x < transform.position.x + attackStats.GetOffset(state.isRight).x - attackStats.Size.x / 2)
            {
                movement_H = -1;
            }
            Debug.Log("D" + movement_H);
        }
        Debug.Log("D" + EnemiesInSight.ToArray().Length);
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
        if (state.IsDead) return;
        if (attackStats == null) return;
        if (attackStats.HasEnemies(transform.position, state.isRight, state.alignment.enemyFactions).Length > 0)
        {
            DoAttack();
        }
    }
    protected override void OnHit(Hit hit)
    {
        base.OnHit(hit);
        if (hit.isRight != state.isRight)
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

