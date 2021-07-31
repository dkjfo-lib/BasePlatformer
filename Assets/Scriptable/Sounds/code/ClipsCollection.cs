using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundCollection", menuName = "MySounds/Collection")]
public class ClipsCollection : ScriptableObject
{
    public SoundType type;
    [Space]
    public AudioClip[] clips;

    public AudioClip GetRandomClip()
    {
        int clipId = Random.Range(0, clips.Length);
        return clips[clipId];
    }

    public void PlayRandomClip(Vector2 position = new Vector2(), Transform parent = null)
    {
        var clip = GetRandomClip();
        if (clip == null) throw new System.Exception("No clip found!");
        SoundController.PlaySound(clip, position, parent);
    }
}
