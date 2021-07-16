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
        if (!charState.CanMove) return;
        float movement_H = 0;
        if (playerDetector.detected)
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
        if (attack.CastAttack(transform.position).Length > 0)
        {
            if (charState.CanAttack && attack.CanAttack())
            {
                charState.inAttack = true;
                Anim_SetTrigger("attack");
                StartCoroutine(WaitWhileAttack());
            }
        }
    }
    IEnumerator WaitWhileAttack()
    {
        yield return WaitWhileAnim("b_attack");
        charState.inAttack = false;
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

