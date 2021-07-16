using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public abstract class GraphicalItem : Base
{
    SpriteRenderer spriteRenderer;
    Animator animator;

    protected void Anim_SetBool(string name, bool value) => animator.SetBool(name, value);
    protected void Anim_SetTrigger(string name) => animator.SetTrigger(name);
    protected IEnumerator WaitWhileAnim(string animationName)
    {
        yield return new WaitWhile(() => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationName);
    }


    protected virtual void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Flip_H()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
