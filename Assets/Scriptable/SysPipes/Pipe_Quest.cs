using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Pipes/QuestData")]
public class Pipe_Quest : ScriptableObject
{
    public bool needsUpdate;
    public QuestProgress currentQuest;

    public void SetQuest(QuestProgress newQuest)
    {
        needsUpdate = true;
        currentQuest = newQuest;
    }
}
