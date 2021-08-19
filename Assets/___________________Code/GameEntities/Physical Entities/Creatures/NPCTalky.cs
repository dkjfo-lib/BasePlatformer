using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalky : NPC, IInetractable
{
    public Dialog[] noramalDialog;
    public Dialog[] deadDialog;
    public Pipe_Dialog dialogPipe;

    protected virtual Dialog GetDialog()
    {
        Dialog dialog = null;
        if (state.IsDead)
            dialog = deadDialog[Random.Range(0, deadDialog.Length)];
        else
            dialog = noramalDialog[Random.Range(0, noramalDialog.Length)];
        return dialog;
    }

    public void Inetract(InetractionParams values)
    {
        dialogPipe.SetDialog(GetDialog());
    }
}

public interface IInetractable
{
    void Inetract(InetractionParams values);
}

public class InetractionParams
{
    public Creature character;
}