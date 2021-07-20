using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "My Quest/Quest")]
public class Quest : ScriptableObject
{
    public QuestObjective[] questObjectives;
    public int Length => questObjectives.Length;
}
