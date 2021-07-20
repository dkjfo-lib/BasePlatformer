using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    private static DialogController controller;

    Coroutine currentDialog;
    public float timeForReplica = 4;

    private void Start()
    {
        controller = this;
    }

    public static void StartDialog(Dialog dialog)
    {
        controller.StopDialog();
        controller.currentDialog = controller.StartCoroutine(controller.DisplayDialog(dialog));
    }

    public IEnumerator DisplayDialog(Dialog dialog)
    {
        for (int i = 0; i < dialog.replicas.Length; i++)
        {
            controller.currentReplica = dialog.replicas[i].Sentence;
            yield return new WaitForSeconds(timeForReplica);
        }
    }
    public void StopDialog()
    {
        if (controller.currentDialog != null)
            controller.StopCoroutine(controller.currentDialog);
    }

    string currentReplica;
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 75), currentReplica);
    }
}
