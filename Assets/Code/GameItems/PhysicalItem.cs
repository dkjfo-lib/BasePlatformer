using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class PhysicalItem : GraphicalItem
{
    public PhysicalStats physicalStats;

    public DetectLayer detectGroundLayer;
    new Rigidbody2D rigidbody;
    new Collider2D collider;


    public Vector2 Velocity => rigidbody.velocity;
    protected float Velocity_H
    {
        get => rigidbody.velocity.x;
        set
        {
            rigidbody.velocity = new Vector2(value, rigidbody.velocity.y);
        }
    }
    protected bool OnGround => detectGroundLayer.Detected;

    protected void SetVelocityH(float setSpeed_H)
    {
        rigidbody.velocity = new Vector2(setSpeed_H * Time.fixedDeltaTime, rigidbody.velocity.y);
        ClampVelocity();
    }
    protected void AddVelocityH(float addSpeed_H)
    {
        rigidbody.velocity += new Vector2(addSpeed_H * Time.fixedDeltaTime, 0);
        ClampVelocity();
    }
    public void AddForceH(float addforce_H)
    {
        rigidbody.velocity += new Vector2(addforce_H / physicalStats.Mass * Time.fixedDeltaTime, 0);
        ClampVelocity();
    }
    public void AddForce(Vector2 addforce)
    {
        rigidbody.velocity += new Vector2(addforce.x / physicalStats.Mass, addforce.y / physicalStats.Mass);
        ClampVelocity();
    }
    protected void MultVelocityH(float multSpeed_H)
    {
        rigidbody.velocity = new Vector2(multSpeed_H * rigidbody.velocity.x, rigidbody.velocity.y);
        ClampVelocity();
    }
    protected void SetVelocityV(float speed_V)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed_V);
        ClampVelocity();
    }
    public void SetVelocity(Vector2 speed_V)
    {
        rigidbody.velocity = speed_V;
        ClampVelocity();
    }
    protected void ClampVelocity()
    {
        if (OnGround)
        {
            rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -physicalStats.maxSpeed_H, physicalStats.maxSpeed_H), rigidbody.velocity.y);
        }
        else
        {
            float inAirMaxSpeed = Mathf.Max(physicalStats.MinSpeedInAir_H, Velocity_H);
            rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -inAirMaxSpeed, inAirMaxSpeed), rigidbody.velocity.y);
        }
    }
    protected void DampVelocity()
    {
        float dampValue = OnGround ? physicalStats.Speed_dump_H : physicalStats.Speed_dump_inAir;
        MultVelocityH(dampValue);
    }


    protected override void GetComponents()
    {
        base.GetComponents();
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    protected override void Init()
    {
        base.Init();
        rigidbody.gravityScale = physicalStats.gravity;
    }

    protected override void OnFixedUpdate()
    {
        detectGroundLayer.UpdateDetector(transform.position, isRight);
        DampVelocity();
    }

    public override void Flip_H()
    {
        base.Flip_H();
        collider.offset = new Vector2(-collider.offset.x, collider.offset.y);
    }
    protected override void AddOnDrawGizmos()
    {
        detectGroundLayer.OnGizmos(transform.position, isRight);
    }
}
