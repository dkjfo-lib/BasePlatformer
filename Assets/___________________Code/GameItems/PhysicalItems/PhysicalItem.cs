using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All physical based shit
/// animator contains "idle" "hurt" "die" 
/// playes sounds of getting hit and destroyed
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class PhysicalItem<TStats, TSounds, TState> : GraphicalItem
    where TStats : StatsBase<TSounds>
    where TState : StateItem
    where TSounds : SoundsItem
{
    public TStats stats;
    public TState state;
    public DetectGround detectGroundLayer;

    Rigidbody2D Rigidbody { get; set; }
    CharacterGUI addon_CharacterGUI { get; set; }

    public override bool isRight => state.isRight;
    public bool OnGround => detectGroundLayer.Detected;
    public Faction Faction => state.alignment.faction;

    public ParticleSystem OnHitParticles;

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
            maxSpeed_H = Mathf.Max(stats.physics.minSpeedInAir_H, Velocity_H);
        }
        Rigidbody.velocity = new Vector2(Mathf.Clamp(Rigidbody.velocity.x, -maxSpeed_H, maxSpeed_H), Rigidbody.velocity.y);
    }

    protected void DampVelocity()
    {
        float dampValue = OnGround ? stats.physics.speed_dump : stats.physics.speed_dump_inAir;
        Velocity_H *= dampValue;
    }

    public float GetHit(Hit hit)
    {
        bool wasDead = state.IsDead;
        state.health -= hit.damage;
        Inertia_H += hit.GetForce;
        PlaySound(stats.sounds.onHitSounds);
        if (OnHitParticles != null)
        {
            var newParticle = Instantiate(OnHitParticles,
                transform.position + Vector3.up * .5f,
                hit.isRight
                    ? OnHitParticles.transform.rotation
                    : Quaternion.Euler(180 - OnHitParticles.transform.rotation.eulerAngles.x, OnHitParticles.transform.rotation.eulerAngles.y, OnHitParticles.transform.rotation.eulerAngles.z),
                transform.parent);
            Destroy(newParticle.gameObject, 5);
        }
        if (state.IsDead)
        {
            if (!wasDead)
            {
                UpdateGUI(false);
                QuestController.OnEvent(new EventDescription
                {
                    who = hit.attackerType,
                    didWhat = EventType.kill,
                    toWhom = stats.entityType
                });
                Anim_SetTrigger("die");
                OnDeath(hit);
                PlaySound(stats.sounds.onDeathAudio);
            }
        }
        else
        {
            UpdateGUI(true);
            Anim_SetTrigger("hurt");
            OnHit(hit);
        }
        return hit.force * stats.physics.toughness;
    }
    protected virtual void OnHit(Hit hit) { }
    protected virtual void OnDeath(Hit hit) { }

    protected void UpdateGUI(bool isDisplayed)
    {
        addon_CharacterGUI?.UpdateUI(state, isDisplayed);
    }

    protected void PlaySound(ClipsCollection collection)
    {
        if (collection == null) return;
        if (collection.type == SoundType.UNDEFINED ||
            collection.type == SoundType.Spawnable)
        {
            collection.PlayRandomClip(transform.position, transform);
        }
        else
        {
            var audioSources = GetComponents<AudioSource>();
            AudioSource audioSource = null;
            switch (collection.type)
            {
                case SoundType.HitSound:
                    audioSource = audioSources[0];
                    break;
                case SoundType.Step:
                    audioSource = audioSources[1];
                    break;
                case SoundType.Voice:
                    audioSource = audioSources[2];
                    break;
            }
            var newClip = collection.GetRandomClip();
            PlaySound(audioSource, newClip);
        }
    }


    protected override void GetComponents()
    {
        base.GetComponents();
        Rigidbody = GetComponent<Rigidbody2D>();
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
