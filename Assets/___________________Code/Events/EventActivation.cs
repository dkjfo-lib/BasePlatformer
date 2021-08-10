using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EventActivation : MonoBehaviour
{
    public Pipe_Events pipe_Events;
    [Space]
    public string activationTag = "404";

    int GlobalCount => pipe_Events.GetEventCount(activationTag);
    int localCount;

    private void Start()
    {
        if (activationTag == "404") Debug.LogWarning("Tag: 404", gameObject);
        localCount = GlobalCount;
        OnStart();
        StartCoroutine(WaitForEvent());
    }

    protected virtual void OnStart() { }

    IEnumerator WaitForEvent()
    {
        while (true)
        {
            yield return new WaitUntil(() => localCount != GlobalCount);
            int diff = GlobalCount - localCount;
            for (int i = 0; i < diff; i++)
            {
                Activate();
            }
            localCount = GlobalCount;
        }
    }

    protected abstract void Activate();
}
