using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : CharacterAbstract
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
        if (playerDetector.detected)
        {
            var target = GetClosest(playerDetector.contacts.Select(s => s.transform));
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
        if (movement_H != 0)
        {
            if (OnGround)
            {
                AddVelocityH(movement_H * physicalStats.Acceleration_H);
            }
            else
            {
                AddVelocityH(movement_H * physicalStats.Acceleration_H * physicalStats.Speed_controll_inAir);
            }
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
        if (attack == null) return;
        if (attack.CastAttack(transform.position, charState.isRight).Length > 0)
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

    Transform GetClosest(IEnumerable<Transform> transforms)
    {
        return transforms.First();
    }

    public override void Flip_H()
    {
        base.Flip_H();
        playerDetector.Flip_H();
    }
}

