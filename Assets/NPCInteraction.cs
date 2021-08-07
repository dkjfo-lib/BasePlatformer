using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInetractable
{
    public Pipe_Dialog Pipe_Dialog;
    [Space]
    public Dialog myDialog;

    public void Inetract(InetractionParams values)
    {
        Pipe_Dialog.SetDialog(myDialog);
    }
}