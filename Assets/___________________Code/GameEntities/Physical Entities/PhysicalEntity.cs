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
public abstract class PhysicalEntity<TStats, TSounds, TState> : PhysicalEntityBase
    where TStats : StatsBase<TSounds>
    where TState : StateItem
    where TSounds : SoundsPhysicalItem
{
    public TStats stats;

    public TState state;
    public DetectGround detectGroundLayer;

    CharacterGUI addon_CharacterGUI { get; set; }

    protected CapsuleCollider2D Collider2D { get; private set; }

    protected override bool OnGround => detectGroundLayer.Detected;
    public Vector3 ObjectTop => (Vector3)Collider2D.offset + Vector3.up * Collider2D.size.y / 2 + transform.position;
    public Vector3 ObjectCenter => (Vector3)Collider2D.offset + transform.position;
    public Vector3 ObjectBottom => transform.position;

    public ParticleSystem OnHitParticles;
    public ParticleSystem OnDestroyParticles;

    // PhysicalEntityBase
    protected override float Mass => stats.physics.mass;
    protected override float MaxSpeed_H => stats.physics.maxSpeed_H;
    protected override float MinSpeedAir_H => stats.physics.minSpeedInAir_H;

    // SeeAndHearEntity
    public override bool isRight { get => state.isRight; set => state.isRight = value; }

    // IHittable
    public override Faction Faction => state.alignment.faction;

    protected void DampVelocity()
    {
        float dampValue = OnGround ? stats.physics.speed_dump : stats.physics.speed_dump_inAir;
        Velocity_H *= dampValue;
    }

    public override Vector2 GetHit(Hit hit)
    {
        bool wasDead = state.IsDead;
        var hitForce = hit.GetForce();
        state.health -= Mathf.Max(0, hit.damage - stats.Armour);
        Inertia += hitForce;
        Spawn.SpawnParticles(transform, OnHitParticles, hit.isRight);

        if (state.IsDead)
        {
            if (!wasDead)
            {
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
                Spawn.SpawnParticles(transform, OnDestroyParticles, hit.isRight);
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