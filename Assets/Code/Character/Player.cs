using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : CharacterAbstract
{
    protected override void OnStart()
    {
        base.OnStart();
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
        if (Input.GetKey(KeyCode.D))
            movement_H += 1;
        if (Input.GetKey(KeyCode.A))
            movement_H -= 1;
        if (movement_H != 0)
        {
            float dampValue = OnGround ? physicalStats.Speed_dump_H : physicalStats.Speed_dump_inAir;
            MultVelocityH(1f / dampValue);
            if (OnGround)
            {
                Velocity_H = Mathf.Clamp(Velocity_H + physicalStats.Acceleration_H, 0, physicalStats.MaxSpeed_H);
                SetVelocityH(movement_H * Velocity_H);
                if (Velocity.x < 0 && charState.isRight || Velocity.x > 0 && !charState.isRight)
                    Flip_H();
            }
            else
            {
                AddVelocityH(movement_H * physicalStats.MaxSpeed_H * physicalStats.Speed_controll_inAir);
                if (Velocity.x < 0 && charState.isRight || Velocity.x > 0 && !charState.isRight)
                    Flip_H();
            }
        }
        Anim_SetBool("run", movement_H != 0 && OnGround);
    }
    void Jump()
    {
        if (charState.IsDead) return;
        if (OnGround)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                SetVelocityV(physicalStats.JumpSpeed);
            }
        }
    }
    void Attack()
    {
        if (charState.IsDead) return;
        if (Input.GetMouseButtonDown(0))
        {
            DoAttack("b_attack");
        }
    }
}
