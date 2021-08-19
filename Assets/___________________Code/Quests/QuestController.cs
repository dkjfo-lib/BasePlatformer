using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestController : EventReceiver
{
    public Pipe_Quest pipe_CurrentQuestDescription;
    [Space]
    public List<Quest> allGameQuests;
    public List<QuestProgress> questsInProgress;

    // TODO: monitor all events at any moment is irrelevant => make it possible to change listening ReceivedEvents at runtime
    protected override IEnumerable<string> ReceivedEvents => allGameQuests.SelectMany(s => s.steps.Select(ss => ss.successOnEvent));

    private void StartQuest(Quest quest)
    {
        var newActiveQuest = new QuestProgress();
        questsInProgress.Add(newActiveQuest);
        StartCoroutine(newActiveQuest.StartQuest(this, quest));
        StartCoroutine(MonitorQuestProgress(newActiveQuest));
    }

    IEnumerator MonitorQuestProgress(QuestProgress questProgress)
    {
        yield return new WaitUntil(() => questProgress.isFinished);
        EndQuest(questProgress);
    }

    void EndQuest(QuestProgress questProgress)
    {
        Debug.Log("QUEST END! Success: " + questProgress.isAbandoned);
        var sendEvents = questProgress.isAbandoned ?
            questProgress.quest.onFailEvents :
            questProgress.quest.onSuccessEvents;

        foreach (var eventTag in sendEvents)
        {
            pipe_Events.SendEvent(eventTag);
        }
    }

    protected override void OnEvent(string eventTag)
    {
        throw new System.NotImplementedException();
    }
}
