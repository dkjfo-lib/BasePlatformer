using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialog : MonoBehaviour, IInetractable
{
    public Pipe_Dialog Pipe_Dialog;
    public Pipe_Events Pipe_Events;
    [Space]
    public Dialog myDialog;

    public void Inetract(InetractionParams values)
    {
        if (myDialog != null)
        {
            Pipe_Dialog.SetDialog(myDialog);
            foreach (var tag in myDialog.emittingTags)
            {
                tag.Emit(Pipe_Events);
            }
        }
    }

    public void SetDialog(Dialog dialogSequence)
    {
        myDialog = dialogSequence;
    }
}