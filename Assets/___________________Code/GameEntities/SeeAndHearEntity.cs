using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class SeeAndHearEntity : BaseEntity, IAudiable
{
    public Pipe_SoundsPlay pipe_SoundsPlay;
    public Pipe_SoundsPlay Pipe_SoundsPlay => pipe_SoundsPlay;

    public abstract bool isRight { get; set; }

    protected void Anim_SetBool(string name, bool value) => Animator.SetBool(name, value);
    protected void Anim_SetTrigger(string name) => Animator.SetTrigger(name);
    protected bool Anim_ClipName(string animationName) => Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    protected IEnumerator WaitWhileAnim(string animationName)
    {
        yield return new WaitWhile(() => Anim_ClipName(animationName));
    }

    protected Animator Animator { get; set; }

    protected override void GetComponents()
    {
        Animator = GetComponent<Animator>();
    }
    protected override void Init()
    {
        inited = true;
        Flip_H(isRight);
    }

    protected void PlayAudio(ClipsCollection clips)
    {
        this.PlayAudio(clips, transform.position);
    }

    public virtual void Flip_H(bool faceRight)
    {
        transform.localScale = new Vector3(faceRight ? 1 : -1, 1, 1);
    }
}
