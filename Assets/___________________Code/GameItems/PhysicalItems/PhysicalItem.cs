using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All physical based shit
/// animator contains "idle" "hurt" "die" 
/// playes sounds of getting hit and destroyed
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public abstract class PhysicalItem<TStats, TSounds, TState> : GraphicalItem, IHittable, IPhisical
    where TStats : StatsBase<TSounds>
    where TState : StateItem
    where TSounds : SoundsPhysicalItem
{
    public TStats stats;
    public TState state;
    public DetectGround detectGroundLayer;

    Rigidbody2D Rigidbody { get; set; }
    CharacterGUI addon_CharacterGUI { get; set; }

    CapsuleCollider2D Collider2D { get; set; }

    public override bool isRight => state.isRight;
    public bool OnGround => detectGroundLayer.Detected;
    public Faction Faction => state.alignment.faction;
    public Vector3 ObjectTop => (Vector3)Collider2D.offset + Vector3.up * Collider2D.size.y / 2 + transform.position;
    public Vector3 ObjectCenter => (Vector3)Collider2D.offset + transform.position;
    public Vector3 ObjectBottom=> transform.position;

    public ParticleSystem OnHitParticles;
    public ParticleSystem OnDestroyParticles;

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
        set
        {
            Rigidbody.velocity = new Vector2(value, Rigidbody.velocity.y);
            ClampVelocity();
        }
    }
    public float Velocity_V
    {
        get => Rigidbody.velocity.y;
        set
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, value);
            ClampVelocity();
        }
    }
    public Vector2 Inertia
    {
        get => Rigidbody.velocity * stats.physics.mass;
        set
        {
            Rigidbody.velocity = value / stats.physics.mass;
            ClampVelocity();
        }
    }
    public float Inertia_H
    {
        get => Rigidbody.velocity.x * stats.physics.mass;
        set
        {
            Rigidbody.velocity = new Vector2(value / stats.physics.mass, Rigidbody.velocity.y);
            ClampVelocity();
        }
    }

    protected void ClampVelocity()
    {
        float maxSpeed_H;
        if (OnGround)
        {
            maxSpeed_H = stats.physics.maxSpeed_H;
        }
        else
        {
            maxSpeed_H = Mathf.Max(stats.physics.minSpeedInAir_H, Velocity_H, -Velocity_H);
        }
        Rigidbody.velocity = new Vector2(Mathf.Clamp(Rigidbody.velocity.x, -maxSpeed_H, maxSpeed_H), Rigidbody.velocity.y);
    }

    protected void DampVelocity()
    {
        float dampValue = OnGround ? stats.physics.speed_dump : stats.physics.speed_dump_inAir;
        Velocity_H *= dampValue;
    }

    public Vector2 GetHit(Hit hit)
    {
        bool wasDead = state.IsDead;
        var hitForce = hit.GetForce(ObjectCenter);
        state.health -= Mathf.Max(0, hit.damage - stats.Armour);
        Inertia += hitForce;
        BaseExt.SpawnParticles(transform, OnHitParticles, hit.isRight);

        if (state.IsDead)
        {
            if (!wasDead)
            {
                QuestController.OnEvent(new EventDescription
                {
                    who = hit.attackerType,
                    didWhat = EventType.kill,
                    toWhom = stats.entityType
                });
                UpdateGUI(false);
                Anim_SetTrigger("die");
                PlayAudio(stats.sounds.onDeathAudio);
                OnDeath(hit);
            }
            else
            {
                PlayAudio(stats.sounds.onHitSounds);
            }
            // Destroy object
            if (state.health < -stats.maxHealth)
            {
                BaseExt.SpawnParticles(transform, OnDestroyParticles, hit.isRight);
                Destroy(gameObject);
            }
        }
        else
        {
            UpdateGUI(true);
            Anim_SetTrigger("hurt");
            PlayAudio(stats.sounds.onHitSounds);
            OnHit(hit);
        }
        return -hitForce * stats.physics.toughness;
    }
    protected virtual void OnHit(Hit hit) { }
    protected virtual void OnDeath(Hit hit) { }

    public void UpdateGUI(bool isDisplayed)
    {
        addon_CharacterGUI?.UpdateUI(state, isDisplayed);
    }

    protected override void GetComponents()
    {
        base.GetComponents();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<CapsuleCollider2D>();
        addon_CharacterGUI = GetComponentInChildren<CharacterGUI>();
    }
    protected override void Init()
    {
        base.Init();
        state.Init(stats.maxHealth);
        Rigidbody.gravityScale = stats.physics.gravity.value;
        Animator.runtimeAnimatorController = stats.animator;
        addon_CharacterGUI?.Init(stats.maxHealth, state);
    }

    protected override void OnFixedUpdate()
    {
        detectGroundLayer.UpdateDetector(transform.position, isRight);
        DampVelocity();
    }

    public override void Flip_H(bool faceRight)
    {
        base.Flip_H(faceRight);
    }
    protected override void AddOnDrawGizmos()
    {
        detectGroundLayer.OnGizmos(transform.position, isRight);
    }
}

public interface IPhisical
{
    Vector2 Velocity { get; set; }
    Vector2 Inertia { get; set; }
}