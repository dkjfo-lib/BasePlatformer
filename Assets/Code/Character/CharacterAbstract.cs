using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DetectLayer))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class CharacterAbstract : MonoBehaviour
{
    public CharStats charStats;
    public CharState charState;
    public Attack attack;

    Rigidbody2D rigidbody;
    DetectLayer detectLayer;
    SpriteRenderer spriteRenderer;
    Animator animator;

    public SoundCollection hitSounds;
    public SoundCollection attackScreams;
    public SoundCollection hitScreams;
    public SoundCollection deathScreams;

    protected bool OnGround => detectLayer.detected;
    protected Vector2 Velocity => rigidbody.velocity;

    private void Start()
    {
        charState.health = charStats.MaxHealth;
        attack.Init();
        GetComponents();
        OnStart();
    }
    private void GetComponents()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        detectLayer = GetComponents<DetectLayer>()[0];
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    protected virtual void OnStart() { }

    private void FixedUpdate()
    {
        DampVelocity();
        OnFixedUpdate();
    }
    protected virtual void OnFixedUpdate() { }

    protected void DampVelocity()
    {
        if (OnGround)
        {
            charState.speed_H = Mathf.Clamp(charState.speed_H - charStats.Movement.Acceleration_H, 0, charStats.Movement.MaxSpeed_H);
            MultVelocityH(charStats.Movement.Speed_dump_H);
        }
        else
        {
            MultVelocityH(charStats.Movement.Speed_dump_inAir);
        }
    }
    protected void SetVelocityH(float setSpeed_H)
    {
        rigidbody.velocity = new Vector2(setSpeed_H * Time.fixedDeltaTime, rigidbody.velocity.y);
    }
    protected void AddVelocityH(float addSpeed_H)
    {
        rigidbody.velocity += new Vector2(addSpeed_H * Time.fixedDeltaTime, 0);
        if (OnGround)
        {
            rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -charStats.Movement.maxSpeed_H, charStats.Movement.maxSpeed_H), rigidbody.velocity.y);
        }
        else
        {
            float inAirMaxSpeed = Mathf.Max(charStats.Movement.MinSpeedInAir_H, charState.speed_H);
            rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -inAirMaxSpeed, inAirMaxSpeed), rigidbody.velocity.y);
        }
    }
    protected void MultVelocityH(float multSpeed_H)
    {
        rigidbody.velocity = new Vector2(multSpeed_H * rigidbody.velocity.x, rigidbody.velocity.y);
    }
    protected void SetVelocityV(float speed_V)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed_V);
    }

    protected void Anim_SetBool(string name, bool value) => animator.SetBool(name, value);
    protected void Anim_SetTrigget(string name) => animator.SetTrigger(name);

    public void GetHit(Hit hit)
    {
        charState.health -= hit.damage;
        if (hit.position.x < transform.position.x)
            AddVelocityH(1000);
        else
            AddVelocityH(-1000);
        if (charState.isDead)
        {
            Anim_SetTrigget("die");
        }
        else
        {
            Anim_SetTrigget("hurt");
        }
    }

    public void PlayHitSound() => hitSounds.PlayRandomClip();
    public void PlayAttackScream() => attackScreams.PlayRandomClip();
    public void PlayHitScream() => hitScreams.PlayRandomClip();
    public void PlayDeathScream() => deathScreams.PlayRandomClip();
    public void PlaySound(SoundCollection collection)
    {
        collection.PlayRandomClip();
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected void Flip_H()
    {
        charState.isRight = !charState.isRight;
        detectLayer.Flip_H();
        spriteRenderer.flipX = !spriteRenderer.flipX;
        if (attack != null) attack.Flip_H();
        AddFlip_H();
    }
    protected virtual void AddFlip_H() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (attack.stats != null) Gizmos.DrawWireCube(transform.position + (Vector3)attack.offset, attack.stats.size);
        AddOnDrawGizmos();
    }
    protected virtual void AddOnDrawGizmos() { }
}