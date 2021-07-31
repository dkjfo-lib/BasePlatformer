using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public abstract class GraphicalItem : Base
{
    public abstract bool isRight { get; }

    SpriteRenderer spriteRenderer;
    protected Animator Animator { get; private set; }

    protected void Anim_SetBool(string name, bool value) => Animator.SetBool(name, value);
    protected void Anim_SetTrigger(string name) => Animator.SetTrigger(name);
    protected IEnumerator WaitWhileAnim(string animationName)
    {
        yield return new WaitWhile(() => Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationName);
    }


    protected override void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }
    protected override void Init()
    {
        inited = true;
        Flip_H(isRight);
    }

    public virtual void Flip_H(bool faceRight)
    {
        transform.localScale = new Vector3(faceRight ? 1 : -1, 1, 1);
    }
}
