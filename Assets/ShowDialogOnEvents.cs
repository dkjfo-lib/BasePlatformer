using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowDialogOnEvents : EventReceiver
{
    public Pipe_Dialog Pipe_Dialog;
    public DialogOnEvent[] dialogsToEvents;

    protected override IEnumerable<string> ReceivedEvents => dialogsToEvents.Select(s => s.eventTag);

    protected override void OnEvent(string eventTag)
    {
        Pipe_Dialog.SetDialog(dialogsToEvents.First(s => s.eventTag == eventTag).dialog);
    }
}
