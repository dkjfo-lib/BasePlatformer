using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialogs/Dialog")]
public class Dialog : ScriptableObject
{
    public TagEmitting[] emittingTags;
    [Space]
    public ReplicaBase[] replicas;
}

[System.Serializable]
public class ReplicaBase 
{
    public Picture authorImage;
    public Line authorName;
    [TextArea]
    public string sentence;
}
