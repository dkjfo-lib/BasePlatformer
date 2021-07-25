using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Girl : NPCTalky
{
    public Dupe[] dupes;
    public Pipe_Quest pipe_Quest;

    protected override Dialog GetDialog()
    {
        Dialog dialog = null;
        if (charState.IsDead)
        {
            dialog = deadDialog;
        }
        else
        {
            if (pipe_Quest.currentQuest != null)
            {
                var dupe = dupes.FirstOrDefault(s => s.quest == pipe_Quest.currentQuest.questDescription);
                if (dupe != null)
                {
                    dialog = dupe.dialog;
                }
                else
                {
                    dialog = noramalDialog;
                }
            }
            else
            {
                dialog = noramalDialog;
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