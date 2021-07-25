using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : CharacterAbstract
{
    public string attackAnimName = "b_attack";

    DetectLayer playerDetector;
    protected override void GetComponents()
    {
        base.GetComponents();
        playerDetector = GetComponents<DetectLayer>()[1];
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Attack();
        Move();
        Jump();
    }

    void Move()
    {
        if (!charState.CanMove) return;
        float movement_H = 0;
        if (playerDetector.Detected)
        {
            var enemies = playerDetector.contacts.
                Select(s => s.gameObject.GetComponent<CharacterAbstract>()).
                Where(s => s != null).
                Where(s => enemyFactions.Contains(s.faction)).
                Select(s => s.transform);
            var target = GetClosest(enemies);
            if (target != null)
            {
                if (target.position.x > transform.position.x && !charState.isRight)
                    Flip_H();
                if (target.position.x < transform.position.x && charState.isRight)
                    Flip_H();
                if (target.position.x > transform.position.x + attackStats.GetOffset(charState.isRight).x + attackStats.size.x / 2)
                {
                    movement_H = +1;
                }
                if (target.position.x < transform.position.x + attackStats.GetOffset(charState.isRight).x - attackStats.size.x / 2)
                {
                    movement_H = -1;
                }
            }
        }
        if (movement_H != 0)
        {
            var addVelocity = OnGround ?
                movement_H * physicalStats.Acceleration_H :
                movement_H * physicalStats.Acceleration_H * physicalStats.Speed_controll_inAir;
            AddVelocityH(addVelocity);
        }
        Anim_SetBool("run", movement_H != 0 && OnGround);
    }
    void Jump()
    {
        if (charState.IsDead) return;
        //if (OnGround)
        //    if (Input.GetKeyDown(KeyCode.W))
        //        SetVelocityV(speed_V);
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
        if (hit.position.x < transform.position.x && charState.isRight)
        {
            Flip_H();
        }
        if (hit.position.x > transform.position.x && !charState.isRight)
        {
            Flip_H();
        }
    }

    public override void Flip_H()
    {
        base.Flip_H();
        playerDetector.Flip_H();
    }
}

