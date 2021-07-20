using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventDescription
{
    public ObjectType who;
    public EventType didWhat;
    public ObjectType toWhom;

    public static bool ChecksOut(EventDescription questEvent, EventDescription happenedEvent)
    {
        return
            (questEvent.who == happenedEvent.who || questEvent.who == ObjectType.UNDEFINED) &&
            (questEvent.didWhat == happenedEvent.didWhat || questEvent.didWhat == EventType.UNDEFINED) &&
            (questEvent.toWhom == happenedEvent.toWhom || questEvent.toWhom == ObjectType.UNDEFINED);
    }

    public override string ToString()
    {
        return $"{who} {didWhat} {toWhom}";
    }
}
