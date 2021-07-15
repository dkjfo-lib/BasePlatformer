using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : CharacterAbstract
{
    protected override void OnFixedUpdate()
    {
        Attack();
        Move();
        Jump();
    }

    void Move()
    {
        float movement_H = 0;
        if (!charState.isDead)
        {
            if (Input.GetKey(KeyCode.D))
                movement_H += 1;
            if (Input.GetKey(KeyCode.A))
                movement_H -= 1;
        }
        if (movement_H != 0)
        {
            if (OnGround)
            {
                charState.speed_H = Mathf.Clamp(charState.speed_H + charStats.Movement.Acceleration_H, 0, charStats.Movement.MaxSpeed_H);
                SetVelocityH(movement_H * charState.speed_H);
                if (Velocity.x < 0 && charState.isRight || Velocity.x > 0 && !charState.isRight)
                    Flip_H();
            }
            else
            {
                AddVelocityH(movement_H * charStats.Movement.MaxSpeed_H * charStats.Movement.Speed_controll_inAir);
                if (Velocity.x < 0 && charState.isRight || Velocity.x > 0 && !charState.isRight)
                    Flip_H();
            }
        }
        Anim_SetBool("run", movement_H != 0 && OnGround);
    }
    void Jump()
    {
        if (charState.isDead) return;
        if (OnGround)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                SetVelocityV(charStats.Movement.JumpSpeed);
            }
        }
    }
    void Attack()
    {
        if (charState.isDead) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (attack.DoAttack(transform.position))
            {
                Anim_SetTrigget("attack");
            }
        }
    }
}
