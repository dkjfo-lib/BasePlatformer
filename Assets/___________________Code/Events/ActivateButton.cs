using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivateButton : EventReceiver
{
    public string[] activationEvents;
    public string[] deactivationEvents;
    Collider2D collider;

    protected override IEnumerable<string> ReceivedEvents => activationEvents.Concat(deactivationEvents);

    protected override void GetComponents()
    {
        collider = GetComponent<Collider2D>();
    }

    protected override void OnEvent(string eventTag)
    {
        if (activationEvents.Contains(eventTag)) collider.enabled = true;
        if (deactivationEvents.Contains(eventTag)) collider.enabled = false;
    }
}
