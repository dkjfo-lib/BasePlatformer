using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InteractableDialog))]
public class SetDialogOnEvent : EventActivation
{
    public DialogEvent[] dialogsEvents;
    protected override string[] ActivationEvents => dialogsEvents.Select(s => s.eventTag).ToArray();

    InteractableDialog interactableDialog;

    protected override void GetComponents()
    {
        interactableDialog = GetComponent<InteractableDialog>();
    }

    protected override void Activate(string eventTag)
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