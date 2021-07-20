using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogVariant", menuName = "My/Dialog/DialogVariant")]
public class ReplicaRandom : ReplicaBase
{
    public string[] sentenceVariant;
    public override string Sentence => sentenceVariant[Random.Range(0, sentenceVariant.Length)];
}
