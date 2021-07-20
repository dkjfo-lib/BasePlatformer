using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    private static DialogController controller;

    IReplica currentReplica = new Replica();
    public float timeForReplica = 4;

    private void Start()
    {
        controller = this;
    }

    public static void StartDialog(Dialog dialog)
    {
        controller.StartCoroutine(controller.DisplayDialog(dialog));
    }

    public IEnumerator DisplayDialog(Dialog dialog)
    {
        for (int i = 0; i < dialog.replicas.Length; i++)
        {
            controller.currentReplica = dialog.replicas[i];
            yield return new WaitForSeconds(timeForReplica);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 75), currentReplica.Sentence);
    }
}
