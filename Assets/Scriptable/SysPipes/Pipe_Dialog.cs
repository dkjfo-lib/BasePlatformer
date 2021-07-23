using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Pipes/DialogData")]
public class Pipe_Dialog : ScriptableObject
{
    public bool needsUpdate;
    public ReplicaBase replica;

    public void SetText(ReplicaBase newReplica)
    {
        needsUpdate = true;
        replica = newReplica;
    }
}
