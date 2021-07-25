using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "My/Dialog/Dialog")]
public class Dialog : ScriptableObject
{
    public ReplicaBase[] replicas;
}

[System.Serializable]
public class ReplicaBase 
{
    public string authorName;
    [TextArea]
    public string sentence;
}
