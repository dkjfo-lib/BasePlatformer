using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalky : NPC, IActivate
{
    public Dialog[] noramalDialog;
    public Dialog[] deadDialog;
    public Pipe_Dialog dialogPipe;

    protected virtual Dialog GetDialog()
    {
        Dialog dialog = null;
        if (charState.IsDead)
            dialog = deadDialog[Random.Range(0, deadDialog.Length)];
        else
            dialog = noramalDialog[Random.Range(0, noramalDialog.Length)];
        return dialog;
    }

    public void Activate(ActivationParams values)
    {
        QuestController.OnEvent(new EventDescription
        {
            who = values.character.characterType,
            didWhat = EventType.talk,
            toWhom = characterType
        });
        dialogPipe.SetText(GetDialog());
    }
}

public interface IActivate
{
    void Activate(ActivationParams values);
}

public class ActivationParams
{
    public CharacterAbstract character;
}