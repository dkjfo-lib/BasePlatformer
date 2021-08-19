using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EventReceiver : BaseEntity
{
    public Pipe_Events pipe_Events;

    protected abstract IEnumerable<string> ReceivedEvents { get; }
    int GlobalCount(string eventTag) => pipe_Events.GetEventCount(eventTag);

    protected override void Init()
    {
        OnInit();
        string[] activationTags = ReceivedEvents.ToArray();
        if (activationTags.Length == 0) Debug.LogWarning("Empty event tags", gameObject);
        foreach (var eventTag in activationTags)
        {
            StartCoroutine(WaitForEvent(eventTag));
        }
    }

    protected virtual void OnInit() { }

    IEnumerator WaitForEvent(string eventTag)
    {
        int localCount = GlobalCount(eventTag);
        while (true)
        {
            yield return new WaitUntil(() => localCount != GlobalCount(eventTag));
            OnEvent(eventTag);
            localCount = GlobalCount(eventTag);
        }
    }

    protected abstract void OnEvent(string eventTag);
}
