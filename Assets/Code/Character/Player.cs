using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : CharacterAbstract
{
    public Vector2 activationSize = Vector2.one;
    public Vector2 activationOffset = Vector2.zero;
    public LayerMask activationLayerMask;

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Attack();
        Move();
        Jump();
        Activate();
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
            if (Input.GetKey(KeyCode.W))
            {
                SetVelocityV(physicalStats.JumpSpeed);
            }
        }
    }
    void Attack()
    {
        if (charState.IsDead) return;
        if (Input.GetMouseButton(0))
        {
            DoAttack("b_attack");
        }
    }
    void Activate()
    {
        if (charState.IsDead) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            var items = Physics2D.OverlapBoxAll(transform.position, activationSize, 0, activationLayerMask);
            var activatesA = items.Select(s => s.transform.GetComponent<IActivate>()).Where(s => s != null).ToArray();
            var activatesAT = items.Select(s => s.transform);
            var tr = GetClosest(activatesAT);
            tr.GetComponent<IActivate>().Activate(new ActivateValues());
        }
    }
    protected override void AddOnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)activationOffset, activationSize);
    }
}
