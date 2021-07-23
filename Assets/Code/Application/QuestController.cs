using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    private static QuestController controller;

    public List<QuestProgress> questsInProgress;
    private QuestProgress displayedQuest;

    public Pipe_Quest pipe_Quest;

    private void Start()
    {
        controller = this;
        foreach (var quest in questsInProgress)
        {
            StartQuest_local(quest);
        }
    }

    public static void AddQuest(Quest quest)
    {
        controller.AddQuest_local(quest);
    }
    private void AddQuest_local(Quest quest)
    {
        questsInProgress.Add(new QuestProgress(quest));
        StartQuest_local(questsInProgress.Last());
    }

    private void StartQuest_local(QuestProgress questProgress)
    {
        StartCoroutine(questProgress.ActiveQuest());
        displayedQuest = questProgress;
        pipe_Quest.SetQuest(displayedQuest);
    }

    public static void EndQuest(QuestProgress quest)
    {
        controller.EndQuest_local(quest);
    }
    public void EndQuest_local(QuestProgress quest)
    {
        // TODO: GiveReward();
        // TODO: DisplayUIMessage();
        questsInProgress.Remove(quest);
        if (displayedQuest == quest)
        {
            displayedQuest = questsInProgress.LastOrDefault();
            pipe_Quest.SetQuest(displayedQuest);
        }
    }

    public static void OnEvent(EventDescription eventDescription)
    {
        controller.OnEvent_local(eventDescription);
    }
    public void OnEvent_local(EventDescription eventDescription)
    {
        foreach (var activeQuest in questsInProgress)
        {
            activeQuest.CheckOut(eventDescription);
        }
    }

    private void OnGUI()
    {
        string message = displayedQuest == null ? null :
            displayedQuest.CurrentStepDescription;
        GUI.Label(new Rect(10 + 75, 10 + 75, 300, 75), message);
    }
}
