using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInetractable
{
    public Pipe_Dialog Pipe_Dialog;
    [Space]
    public Dialog myDialog;

    Creature me;

    private void Start()
    {
        me = GetComponent<Creature>();
    }

    public void Inetract(InetractionParams values)
    {
        QuestController.OnEvent(new EventDescription
        {
            who = values.character.stats.entityType,
            didWhat = EventType.talk,
            toWhom = me.stats.entityType
        });
        Pipe_Dialog.SetDialog(myDialog);
    }
}