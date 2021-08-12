using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Door : EventReceiver
{
    public string[] OpenEvents;
    public string[] CloseEvents;
    protected override IEnumerable<string> ReceivedEvents => OpenEvents.Concat(CloseEvents);
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

    protected override void OnEvent(string eventTag)
    {
        if (OpenEvents.Contains(eventTag))
        {
            if (open == true) return;
            SetIsOpen(true);
            PlayAudio(addon_openSound);
        }
        if (CloseEvents.Contains(eventTag))
        {
            if (open == false) return;
            SetIsOpen(false);
            PlayAudio(addon_closeSound);
        }
    }

    void SetIsOpen(bool value)
    {
        open = value;
        collider.enabled = !value;
        animator.SetBool("active", value);
    }
}
