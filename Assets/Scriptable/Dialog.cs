using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "My/Dialog")]
public class Dialog : ScriptableObject
{
    public IReplica[] replicas;
}

public interface IReplica
{
    public string Sentence { get; }
}

[System.Serializable]
public class Replica : IReplica
{
    public string sentence;
    public string Sentence => sentence;
}

[System.Serializable]
public class ReplicaRandom : IReplica
{
    public string[] sentenceVariant;
    public string Sentence => sentenceVariant[Random.Range(0, sentenceVariant.Length)];
}
