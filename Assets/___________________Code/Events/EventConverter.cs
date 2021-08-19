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
        var triggeredOrigin = eventToEvents.First(s => s.eventTagOriginal == eventTag);
        foreach (var newEventTag in triggeredOrigin.eventTagResults)
        {
            pipe_Events.SendEvent(newEventTag);
        }
    }
}

[System.Serializable]
public struct EventToEvent
{
    public string eventTagOriginal;
    public string[] eventTagResults;
}