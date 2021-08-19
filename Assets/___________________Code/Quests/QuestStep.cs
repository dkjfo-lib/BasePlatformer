using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjective", menuName = "My Quest/Objective")]
public class QuestStep : ScriptableObject
{
    public string successOnEvent;
    public int timesNeeded = 1;
    [TextArea(minLines: 3, maxLines: 5)]
    public string description;
    public string Description => string.IsNullOrWhiteSpace(description) ?
       string.Join(" ", successOnEvent, timesNeeded, "times") : description;
}
