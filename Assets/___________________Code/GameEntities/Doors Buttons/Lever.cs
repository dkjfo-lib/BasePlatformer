using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : SeeAndHearEntity, IInetractable
{
    public Pipe_Events pipe_Events;
    [Space]
    [Tooltip("Trigger Levers are only activating and not deactivating ")]
    public bool isTriggerLever = false;
    public bool isActive = false;
    [Space]
    public string[] onActivationEvents;
    public string[] onDeactivationEvents;
    [Space]
    [Space]
    public ClipsCollection addon_pressSound;

    public override bool isRight { get; set; }

    protected override void Init()
    {
        if (onActivationEvents.Length == 0) Debug.LogWarning("Empty tags!", gameObject);
        if (isActive) SendEvent(true);
    }

    public void Inetract(InetractionParams values)
    {
        isActive = !isActive || isTriggerLever;
        PlayAudio(addon_pressSound);
        SendEvent(isActive || isTriggerLever);
    }

    void SendEvent(bool value)
    {
        Anim_SetBool("active", value);
        if (value)
        {
            foreach (var eventTag in onActivationEvents)
            {
                pipe_Events.SendEvent(eventTag);
            }
        }
        else
        {
            foreach (var eventTag in onDeactivationEvents)
            {
                pipe_Events.SendEvent(eventTag);
            }
        }
    }
}