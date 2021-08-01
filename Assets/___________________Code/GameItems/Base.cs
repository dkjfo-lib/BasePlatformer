using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
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


    protected virtual void AddOnDrawGizmos() { }

    protected void PlaySound(AudioSource audioSource, AudioClip newClip)
    {
        if (audioSource.clip == newClip)
        {
            if (audioSource.isPlaying)
                audioSource.time = 0;
            else
                audioSource.Play();
        }
        else
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}