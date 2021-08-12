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
    [TextArea(4, 10)]
    public string sentence;
}
