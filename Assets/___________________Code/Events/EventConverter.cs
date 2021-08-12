using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventConverter : EventReceiver
{
    public EventToEvent[] eventToEvents;
    protected override IEnumerable<string> ReceivedEvents => eventToEvents.Select(s => s.eventTagOriginal);

    protected override void OnEvent(string eventTag)
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