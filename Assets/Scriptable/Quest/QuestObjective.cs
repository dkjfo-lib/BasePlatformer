using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjective", menuName = "My Quest/Objective")]
public class QuestObjective : ScriptableObject
{
    public EventDescription eventDescription;
    public int times = 1;
    public string description;
    public bool NoDescription => string.IsNullOrWhiteSpace(description);
    public string Description => NoDescription ?
       string.Join(" ", eventDescription.ToString(), times, "times") : description;
}
