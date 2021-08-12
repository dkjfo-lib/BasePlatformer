using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InteractableDialog))]
public class SetDialogOnEvent : EventReceiver
{
    public DialogEvent[] dialogsEvents;
    protected override string[] ReceivedEvents => dialogsEvents.Select(s => s.eventTag).ToArray();

    InteractableDialog interactableDialog;

    protected override void GetComponents()
    {
        interactableDialog = GetComponent<InteractableDialog>();
    }

    protected override void OnEvent(string eventTag)
    {
        interactableDialog.SetDialog(dialogsEvents.First(s => s.eventTag == eventTag).dialogSequence);
    }
}

[System.Serializable]
public struct DialogEvent
{
    public DialogSequence dialogSequence;
    public string eventTag;
}