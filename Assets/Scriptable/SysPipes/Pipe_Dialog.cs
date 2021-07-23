using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Pipes/DialogData")]
public class Pipe_Dialog : ScriptableObject
{
    public bool needsUpdate;
    public Dialog dialog;

    public void SetText(Dialog newDialog)
    {
        needsUpdate = true;
        dialog = newDialog;
    }
}
