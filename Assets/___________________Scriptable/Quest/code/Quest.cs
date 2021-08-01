using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "My Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public QuestObjective[] questObjectives;
    public int Length => questObjectives.Length;
    public Quest[] NextQuests;
}
