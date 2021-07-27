using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public abstract class GraphicalItem : Base
{
    public abstract bool isRight { get; }

    SpriteRenderer spriteRenderer;
    Animator animator;

    protected void Anim_SetBool(string name, bool value) => animator.SetBool(name, value);
    protected void Anim_SetTrigger(string name) => animator.SetTrigger(name);
    protected IEnumerator WaitWhileAnim(string animationName)
    {
        yield return new WaitWhile(() => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationName);
    }


    protected override void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    protected override void Init()
    {
        inited = true;
    }

    public virtual void Flip_H()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
