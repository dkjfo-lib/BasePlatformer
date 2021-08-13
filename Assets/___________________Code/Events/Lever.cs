using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Base, IInetractable
{
    public Pipe_Events pipe_Events;
    [Space]
    public bool active = false;
    public TagEmitting[] activationTags;
    public TagEmitting[] deactivationTags;
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
        if (activationTags.Length == 0) Debug.LogWarning("Empty tags!", gameObject);
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
        {
            foreach (var tag in activationTags)
            {
                tag.Emit(pipe_Events);
            }
        }
        else
        {
            foreach (var tag in deactivationTags)
            {
                tag.Emit(pipe_Events);
            }
        }
    }
}

[System.Serializable]
public class TagEmitting
{
    public string eventTag = "404";

    public void Emit(Pipe_Events pipe_Events)
    {
        pipe_Events.SendEvent(eventTag);
    }
}