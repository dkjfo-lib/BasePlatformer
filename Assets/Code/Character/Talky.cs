using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talky : CharacterAbstract, IActivate
{
    public Dialog dialog;

    public void Activate(ActivateValues values)
    {
        DialogController.StartDialog(dialog);
    }
}

public interface IActivate
{
    void Activate(ActivateValues values);
}

public class ActivateValues
{
    public CharacterAbstract character;
}