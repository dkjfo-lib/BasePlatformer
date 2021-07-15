using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DetectLayer))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public float maxSpeed_H = 600;
    public float speed_H = 0;
    public float acceleration = 30;
    public float minSpeedInAir = 6;
    [HideInInspector] public bool isRight = true;
    [Space]
    public int hp = 10;
    [Space]
    public bool dead = false;

    Rigidbody2D rigidbody;
    DetectLayer detectLayer;
    SpriteRenderer spriteRenderer;
    Animator animator;

    protected bool OnGround => detectLayer.detected;
    protected Vector2 Velocity => rigidbody.velocity;

    protected void GetComponents()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        detectLayer = GetComponents<DetectLayer>()[0];
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
            rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -maxSpeed_H, maxSpeed_H), rigidbody.velocity.y);
        }
        else
        {
            float inAirMaxSpeed = Mathf.Max(minSpeedInAir, speed_H);
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
        hp -= hit.damage;
        if (hit.position.x < transform.position.x)
            AddVelocityH(1000);
        else
            AddVelocityH(-1000);
        if (!dead)
        {
            if (hp < 1)
            {
                dead = true;
                Anim_SetTrigget("die");
            }
            else
            {
                Anim_SetTrigget("hurt");
            }
        }
    }

    public void PlaySound(AudioClip sound)
    {
        SoundController.PlaySound(sound);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected void Flip_H()
    {
        isRight = !isRight;
        detectLayer.Flip_H();
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
