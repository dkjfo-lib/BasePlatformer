using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    AudioSource audioSource;

    public SoundCollection currentPlaylist;
    public bool playOnStart = true;
    public bool play = false;
    public float seconds = 1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (playOnStart) PlaySongs();
    }

    public void PlaySongs(SoundCollection music)
    {
        currentPlaylist = music;
        PlaySongs();
    }
    public void PlaySongs()
    {
        StartCoroutine(StartPlayer());
    }

    IEnumerator StartPlayer()
    {
        play = true;
        while (play)
        {
            audioSource.clip = currentPlaylist.GetRandomClip();
            audioSource.Play();
            if (audioSource.volume != 1) yield return UnmutePlayer();
            yield return new WaitWhile(() => play && audioSource.isPlaying);
        }
        yield return MutePlayer();
        audioSource.Stop();
    }


    IEnumerator UnmutePlayer()
    {
        int steps = 8;
        while (audioSource.volume != 1)
        {
            audioSource.volume += 1f / steps;
            yield return new WaitForSeconds(seconds / steps);
        }
        audioSource.volume = 1;
    }
    IEnumerator MutePlayer()
    {
        int steps = 8;
        audioSource.volume = 1;
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForSeconds(seconds / steps);
            audioSource.volume -= seconds / steps;
        }
        audioSource.volume = 0;
    }
}
