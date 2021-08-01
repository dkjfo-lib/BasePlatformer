using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Creature
{
    public static Player thePlayer;
    public static bool respawn = true;

    public DetectActive activationRect;
    public override Vector2 LimbsDirection
    {
        get
        {
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;
            return direction;
        }
    }

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
        if (state.IsDead) return;

        Attack();
        Move();
        Jump();
        Activate();
    }

    void Move()
    {
        int movement_H = 0;
        if (Input.GetKey(KeyCode.D))
        {
            movement_H += 1;
            //if (!isRight) Flip_H(true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement_H -= 1;
            //if (isRight) Flip_H(false);
        }
        if (LimbsDirection.x < 0 && isRight)
            Flip_H(false);
        if (LimbsDirection.x > 0 && !isRight)
            Flip_H(true);
        DoMove(movement_H);
    }
    void Jump()
    {
        if (OnGround)
            if (Input.GetKey(KeyCode.W))
                DoJump();
    }

    void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            DoAttack();
        }
    }
    void Activate()
    {
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
