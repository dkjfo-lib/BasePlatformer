using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicalEntityBase : SeeAndHearEntity, IHittable
{
    // STATS
    public abstract Faction Faction { get; }
    protected abstract float Mass { get; }
    protected abstract float MaxSpeed_H { get; }
    protected abstract float MinSpeedAir_H { get; }

    // STATE
    protected abstract bool OnGround { get; }

    // REFS
    protected Rigidbody2D Rigidbody { get; private set; }

    protected override void GetComponents()
    {
        base.GetComponents();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public abstract Vector2 GetHit(Hit hit);

    #region ______________________________________________________________________________Velocity & Inertia_________________________________
    public Vector2 Velocity
    {
        get => Rigidbody.velocity;
        set
        {
            Rigidbody.velocity = value;
            ClampVelocity();
        }
    }
    public float Velocity_H
    {
        get => Rigidbody.velocity.x;
        set => Velocity = new Vector2(value, Rigidbody.velocity.y);
    }
    public float Velocity_V
    {
        get => Rigidbody.velocity.y;
        set => Velocity = new Vector2(Rigidbody.velocity.x, value);
    }
    public Vector2 Inertia
    {
        get => Velocity * Mass;
        set => Velocity = value / Mass;
    }
    public float Inertia_H
    {
        get => Velocity_H * Mass;
        set => Velocity_H = value / Mass;
    }

    protected void ClampVelocity()
    {
        float maxSpeed_H = OnGround ?
            MaxSpeed_H :
            Mathf.Max(MinSpeedAir_H, Velocity_H, -Velocity_H);
        float newVelH = Mathf.Clamp(Velocity_H, -maxSpeed_H, maxSpeed_H);
        Rigidbody.velocity = new Vector2(newVelH, Velocity_V);
    }
    #endregion
}