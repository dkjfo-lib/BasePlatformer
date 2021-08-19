using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCTalkyByQuest : NPCTalky
{
    public Dupe[] dupes;
    public Pipe_Quest pipe_Quest;

    protected override Dialog GetDialog()
    {
        Dialog dialog = null;
        if (state.IsDead)
        {
            dialog = deadDialog[Random.Range(0, deadDialog.Length)];
        }
        else
        {
            if (pipe_Quest.currentQuest != null)
            {
                var dupe = dupes.FirstOrDefault(s => s.quest == pipe_Quest.currentQuest.quest);
                if (dupe != null)
                {
                    dialog = dupe.dialog;
                }
                else
                {
                    dialog = noramalDialog[Random.Range(0, noramalDialog.Length)];
                }
            }
            else
            {
                dialog = noramalDialog[Random.Range(0, noramalDialog.Length)];
            }
        }
        return dialog;
    }
}

[System.Serializable]
public class Dupe
{
    public Dialog dialog;
    public Quest quest;
}