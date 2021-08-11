using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialog : MonoBehaviour, IInetractable
{
    public Pipe_Dialog Pipe_Dialog;
    public Pipe_Events Pipe_Events;
    [Space]
    public DialogSequence myDialog;
    public bool firstTime = true;

    public void Inetract(InetractionParams values)
    {
        Dialog displayDialog = firstTime ?
            myDialog.firstTimeDialog :
            myDialog.GetLaterDialog();
        if (displayDialog != null)
        {
            firstTime = false;
            Pipe_Dialog.SetDialog(displayDialog);
            foreach (var tag in displayDialog.emittingTags)
            {
                tag.Emit(Pipe_Events);
            }
        }
    }

    public void SetDialog(DialogSequence dialogSequence)
    {
        myDialog = dialogSequence;
        firstTime = true;
    }
}