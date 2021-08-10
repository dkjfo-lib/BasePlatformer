using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EventActivation : Base
{
    public Pipe_Events pipe_Events;
    [Space]
    public string activationTag = "404";

    int GlobalCount => pipe_Events.GetEventCount(activationTag);
    int localCount;

    protected override void Init()
    {
        if (activationTag == "404") Debug.LogWarning("Tag: 404", gameObject);
        localCount = GlobalCount;
        OnInit();
        StartCoroutine(WaitForEvent());
    }

    protected virtual void OnInit() { }

    IEnumerator WaitForEvent()
    {
        while (true)
        {
            yield return new WaitUntil(() => localCount != GlobalCount);
            if (GlobalCount > localCount)
                Activate();
            if (GlobalCount < localCount)
                Deactivate();
            localCount = GlobalCount;
        }
    }

    protected abstract void Activate();
    protected abstract void Deactivate();
}
