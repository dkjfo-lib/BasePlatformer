using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip[] currentPlaylist;
    public bool playOnStart = true;
    public bool play = false;
    public float seconds = 1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (playOnStart) PlaySongs();
    }

    public void PlaySongs(AudioClip[] music)
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
        int songId = 0;
        while (play)
        {
            audioSource.clip = currentPlaylist[songId];
            audioSource.Play();
            if (audioSource.volume != 1) yield return UnmutePlayer();
            yield return new WaitWhile(() => play && audioSource.isPlaying);
            songId = songId + 1 == currentPlaylist.Length ? 0 : songId + 1;
        }
        yield return MutePlayer();
        audioSource.Stop();
    }


    IEnumerator UnmutePlayer()
    {
        int steps = 8;
        audioSource.volume = 0;
        while (audioSource.volume != 1)
        {
            yield return new WaitForSeconds(seconds / steps);
            audioSource.volume += seconds / steps;
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
