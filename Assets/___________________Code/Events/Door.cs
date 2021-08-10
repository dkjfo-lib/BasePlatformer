using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Door : EventActivation
{
    public bool open = false;
    [Space]
    [Space]
    public ClipsCollection addon_openSound;
    public ClipsCollection addon_closeSound;

    new Collider2D collider;
    Animator animator;

    protected override void GetComponents()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void OnInit()
    {
        SetIsOpen(open);
    }

    protected override void Activate()
    {
        if (open == true) return;
        SetIsOpen(true);
        PlayAudio(addon_openSound);
    }

    protected override void Deactivate()
    {
        if (open == false) return;
        SetIsOpen(false);
        PlayAudio(addon_closeSound);
    }

    void SetIsOpen(bool value)
    {
        open = value;
        collider.enabled = !value;
        animator.SetBool("active", value);
    }
}
