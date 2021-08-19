using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    public QuestController controller;
    public Quest quest;

    public QuestStep CurrentStep => quest.steps[currentStepId];
    public int currentStepId;
    public int currentStepTimes;
    public string CurrentStepDescription => CurrentStep.Description;

    private bool stepCompleted = false;
    public bool StepCompleted => stepCompleted;
    public bool notEmpty => quest != null;

    public bool isAbandoned = false;
    public bool isFinished = false;

    private void Init(QuestController controller, Quest quest)
    {
        this.controller = controller;
        this.quest = quest;
        currentStepId = 0;
        currentStepTimes = 0;
        stepCompleted = false;
        isAbandoned = false;
    }

    public IEnumerator StartQuest(QuestController controller, Quest quest)
    {
        Init(controller, quest);
        for (; currentStepId < quest.Length || isAbandoned; currentStepId++)
        {
            yield return new WaitUntil(() => stepCompleted || isAbandoned);
            currentStepTimes = 0;
            stepCompleted = false;
        }
        isFinished = true;
    }

    public void AbandonQuest()
    {
        isAbandoned = true;
    }

    public bool OnEvent(string eventTag)
    {
        bool questEventHappened = eventTag == CurrentStep.successOnEvent;
        if (questEventHappened)
        {
            currentStepTimes++;
        }
        stepCompleted = currentStepTimes >= CurrentStep.timesNeeded;
        return stepCompleted;
    }
}
