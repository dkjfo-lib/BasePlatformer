using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Creature
{
    public static Player thePlayer;
    public static bool respawn = true;

    public DetectActive activationRect;

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
        if (!state.CanMove) return;
        int movement_H = 0;
        if (Input.GetKey(KeyCode.D))
            movement_H += 1;
        if (Input.GetKey(KeyCode.A))
            movement_H -= 1;
        DoMove(movement_H);
    }
    void Jump()
    {
        if (state.IsDead) return;
        if (OnGround)
            if (Input.GetKey(KeyCode.W))
                DoJump();
    }

    void Attack()
    {
        if (state.IsDead) return;
        if (Input.GetMouseButton(0))
        {
            DoAttack();
        }
    }
    void Activate()
    {
        if (state.IsDead) return;
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
        activationRect.OnGizmos(transform.position, isRight);
    }
}
