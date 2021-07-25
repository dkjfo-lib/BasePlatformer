using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    public Pipe_Dialog pipe_Dialog;
    [Space]
    public Image image;
    public Text textAuthorName;
    public Text textSentence;
    public float timeForReplica = 2;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(KeepDialogUpdated());
    }

    IEnumerator KeepDialogUpdated()
    {
        while (true)
        {
            yield return new WaitUntil(() => pipe_Dialog.needsUpdate);
            pipe_Dialog.needsUpdate = false;
            StartCoroutine(DisplayDialog(pipe_Dialog.dialog));
        }
    }

    IEnumerator DisplayDialog(Dialog dialog)
    {
        animator.SetBool("inDialog", true);
        Time.timeScale = 0;
        for (int i = 0; i < dialog.replicas.Length; i++)
        {
            DisplayReplica(dialog.replicas[i]);

            float replicaShownAt = Time.unscaledTime;
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() =>
                Time.unscaledTime - replicaShownAt > timeForReplica || 
                Input.GetMouseButtonDown(0));
        }
        Time.timeScale = 1;
        animator.SetBool("inDialog", false);
    }

    void DisplayReplica(ReplicaBase replica)
    {
        textAuthorName.text = replica.authorName;
        textSentence.text = replica.sentence;
    }
}
