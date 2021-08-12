using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InteractableDialog))]
public class SetDialogOnEvent : EventReceiver
{
    public DialogOnEvent[] dialogsEvents;
    protected override IEnumerable<string> ReceivedEvents => dialogsEvents.Select(s => s.eventTag);

    InteractableDialog interactableDialog;

    protected override void GetComponents()
    {
        interactableDialog = GetComponent<InteractableDialog>();
    }

    protected override void OnEvent(string eventTag)
    {
        interactableDialog.SetDialog(dialogsEvents.First(s => s.eventTag == eventTag).dialog);
    }
}

[System.Serializable]
public struct DialogOnEvent
{
    public Dialog dialog;
    public string eventTag;
}