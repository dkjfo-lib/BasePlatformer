using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Base, IInetractable
{
    public Pipe_Events pipe_Events;
    [Space]
    public bool active = false;
    public string activationTag = "404";
    [Space]
    [Space]
    public ClipsCollection addon_pressSound;

    Animator animator;

    protected override void GetComponents()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Init()
    {
        if (activationTag == "404") Debug.LogWarning("Tag: 404", gameObject);
        if (active) SendEvent(true);
    }

    public void Inetract(InetractionParams values)
    {
        active = !active;
        PlayAudio(addon_pressSound);
        SendEvent(active);
    }

    void SendEvent(bool value)
    {
        animator.SetBool("active", value);
        if (value)
            pipe_Events.AddEvent(activationTag);
        else
            pipe_Events.SubtractEvent(activationTag);
    }
}
