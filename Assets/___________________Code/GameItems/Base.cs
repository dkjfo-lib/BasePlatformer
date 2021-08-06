using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    public Pipe_SoundsPlay soundsPipe;

    public bool inited { get; protected set; } = false;
    private void Start()
    {
        GetComponents();
        Init();
    }
    protected virtual void GetComponents() { }
    protected virtual void Init() { }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }
    protected virtual void OnFixedUpdate() { }

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        AddOnDrawGizmos();
    }

    protected void PlayAudio(ClipsCollection collection)
    {
        if (collection == null) return;

        soundsPipe.awaitingClips.Add(new PlayClipData
        {
            clipCollection = collection,
            position = transform.position
        });
    }

    protected virtual void AddOnDrawGizmos() { }
}