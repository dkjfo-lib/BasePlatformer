using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "My Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [Space]
    public string[] onSuccessEvents;
    public string[] onFailEvents;
    [Space]
    public QuestStep[] steps;

    public int Length => steps.Length;
}
