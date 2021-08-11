using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogSequence", menuName = "Dialogs/DialogSequence")]
public class DialogSequence : ScriptableObject
{
    public Dialog firstTimeDialog;
    public Dialog[] laterDialog;

    public Dialog GetLaterDialog() => laterDialog.Length > 0 ? laterDialog[Random.Range(0, laterDialog.Length)] : null;
}
