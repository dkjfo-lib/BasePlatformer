using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundCollection", menuName = "My/SoundCollection")]
public class ClipsCollection : ScriptableObject
{
    public AudioClip[] clips;

    public AudioClip GetRandomClip()
    {
        int clipId = Random.Range(0, clips.Length);
        return clips[clipId];
    }

    public void PlayRandomClip()
    {
        var clip = GetRandomClip();
        if (clip == null) throw new System.Exception("No clip found!");
        SoundController.PlaySound(clip);
    }
}
