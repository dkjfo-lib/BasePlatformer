using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : CharacterAbstract
{
    DetectLayer playerDetector;
    protected override void OnStart()
    {
        playerDetector = GetComponents<DetectLayer>()[1];
    }

    protected override void OnFixedUpdate()
    {
        Attack();
        Move();
        Jump();
    }

    void Move()
    {
        float movement_H = 0;
        if (!charState.isDead && playerDetector.detected)
        {
            var target = GetClosest(playerDetector.contacts.Select(s => s.transform));
            if (target.position.x > transform.position.x && !charState.isRight)
                Flip_H();
            if (target.position.x < transform.position.x && charState.isRight)
                Flip_H();
            if (target.position.x > transform.position.x + attack.stats.offset.x + attack.stats.size.x / 2)
            {
                movement_H = +1;
            }
            if (target.position.x < transform.position.x + attack.stats.offset.x - attack.stats.size.x / 2)
            {
                movement_H = -1;
            }
        }
        if (movement_H != 0)
        {
            if (OnGround)
            {
                charState.speed_H = Mathf.Clamp(charState.speed_H + charStats.Movement.Acceleration_H, 0, charStats.Movement.MaxSpeed_H);
                SetVelocityH(movement_H * charState.speed_H);
            }
            else
            {
                if (movement_H != 0)
                {
                    AddVelocityH(movement_H * charStats.Movement.MaxSpeed_H * charStats.Movement.Speed_controll_inAir);
                }
            }
        }
    }
    void Jump()
    {
        if (charState.isDead) return;
        //if (OnGround)
        //    if (Input.GetKeyDown(KeyCode.W))
        //        SetVelocityV(speed_V);
    }
    void Attack()
    {
        if (charState.isDead) return;
        if (attack == null) return;
        if (Physics2D.OverlapBox((Vector2)transform.position + attack.stats.offset, attack.stats.size, 0, attack.stats.layerMask))
        {
            if (attack.DoAttack(transform.position))
            {
                Anim_SetTrigget("attack");
            }
        }
    }

    Transform GetClosest(IEnumerable<Transform> transforms)
    {
        return transforms.First();
    }

    protected override void AddFlip_H()
    {
        playerDetector.Flip_H();
    }
}

