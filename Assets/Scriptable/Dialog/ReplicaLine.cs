using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogLine", menuName = "My/Dialog/DialogLine")]
public class ReplicaLine : ReplicaBase
{
    public string sentence;
    public override string Sentence => sentence;
}
