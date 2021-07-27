using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : CharacterAbstract
{
    public static Player thePlayer;
    public static bool respawn = true;

    public MyCastRect activationRect;

    public Pipe_BetweenScenesData pipe_BetweenScenesData;

    private void Awake()
    {
        thePlayer = this;
    }

    protected override void Init()
    {
        base.Init();
        if (!respawn)
        {
            pipe_BetweenScenesData.ApplyData(this);
        }
        respawn = false;
        UpdateGUI(true);
    }

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
            if (movement_H < 0 && isRight || movement_H > 0 && !isRight)
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
            var items = activationRect.Cast(transform.position, isRight);
            var activatesA = items.Select(s => s.transform.GetComponent<IActivate>()).Where(s => s != null).ToArray();
            var activatesAT = items.Select(s => s.transform);
            var tr = GetClosest(activatesAT);
            if (tr != null)
            {
                tr.GetComponent<IActivate>().Activate(new ActivationParams
                {
                    character = this
                });
            }
        }
    }

    protected override void OnDeath(Hit hit)
    {
        base.OnDeath(hit);
        respawn = true;
    }
    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)activationRect.GetOffset(isRight), activationRect.Size);
    }
}
