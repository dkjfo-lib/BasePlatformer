using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventConverter : EventActivation
{
    public EventToEvent[] eventToEvents;
    protected override string[] ActivationEvents => eventToEvents.Select(s => s.eventTagOriginal).ToArray();

    protected override void Activate(string eventTag)
    {
        pipe_Events.AddEvent(eventToEvents.First(s => s.eventTagOriginal == eventTag).eventTagResult);
    }
}

[System.Serializable]
public struct EventToEvent
{
    public string eventTagOriginal;
    public string eventTagResult;
}