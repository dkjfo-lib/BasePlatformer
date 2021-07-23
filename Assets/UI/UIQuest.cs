using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuest : MonoBehaviour
{
    public Pipe_Quest pipe_Quest;
    [Space]
    public Text textQuestName;
    public Text textStepDescription;

    private void Start()
    {
        StartCoroutine(KeepDialogUpdated());
    }

    IEnumerator KeepDialogUpdated()
    {
        while (true)
        {
            yield return new WaitUntil(() => pipe_Quest.needsUpdate || pipe_Quest.currentQuest.StepCompleted);
            pipe_Quest.needsUpdate = false;
            DisplayReplica(pipe_Quest.currentQuest);
        }
    }

    void DisplayReplica(QuestProgress questProgress)
    {
        if (questProgress != null)
        {
            textQuestName.text = questProgress.questDescription.questName;
            textStepDescription.text = questProgress.CurrentStepDescription;
        }
        else
        {
            //TODO: close
            textQuestName.text = "None";
            textStepDescription.text = "";
        }
    }
}
