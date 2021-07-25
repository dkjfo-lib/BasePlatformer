using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public Quest questDescription;
    public int currentStepId;

    public QuestObjective CurrentStep => questDescription.questObjectives[currentStepId];
    public string CurrentStepDescription => CurrentStep.Description;
    public int currentStepHappenedTimes;

    private bool stepCompleted = false;
    public bool StepCompleted => stepCompleted;
    public bool notEmpty => questDescription != null;

    public QuestProgress(Quest questDescription)
    {
        this.questDescription = questDescription;
        currentStepId = 0;
        currentStepHappenedTimes = 0;
    }

    public IEnumerator ActiveQuest()
    {
        for (; currentStepId < questDescription.Length; currentStepId++)
        {
            yield return new WaitUntil(() => stepCompleted);
            currentStepHappenedTimes = 0;
            stepCompleted = false;
        }
        QuestEnd();
    }

    public bool CheckOut(EventDescription eventDescription)
    {
        bool questEventHappened = EventDescription.ChecksOut(CurrentStep.eventDescription, eventDescription);
        if (questEventHappened)
        {
            currentStepHappenedTimes++;
        }
        stepCompleted = currentStepHappenedTimes >= CurrentStep.times;
        return stepCompleted;
    }

    public void QuestEnd()
    {
        Debug.Log("QUEST END!");
        QuestController.EndQuest(this);
        foreach (var nextQuest in questDescription.NextQuests)
        {
            QuestController.AddQuest(nextQuest);
        }
    }
}
