using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : CharacterAbstract
{
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
            var addVelocity = OnGround ?
                movement_H * physicalStats.Acceleration_H :
                movement_H * physicalStats.Acceleration_H * physicalStats.Speed_controll_inAir;
            AddVelocityH(addVelocity);
            if (Velocity_H < 0 && charState.isRight || Velocity_H > 0 && !charState.isRight)
            {
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
