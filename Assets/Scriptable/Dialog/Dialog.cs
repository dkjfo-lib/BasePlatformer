using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "My/Dialog/Dialog")]
public class Dialog : ScriptableObject
{
    public ReplicaBase[] replicas;
}

public abstract class ReplicaBase : ScriptableObject
{
    public string authorName;
    public abstract string Sentence { get; }
}
