using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talky : CharacterAbstract, IActivate
{
    public Dialog dialog;

    public void Activate(ActivationParams values)
    {
        QuestController.OnEvent(new EventDescription
        {
            who = values.character.characterType,
            didWhat = EventType.talk,
            toWhom = characterType
        });
        DialogController.StartDialog(dialog);
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