using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public Pipe_SoundsPlay pipe;
    [Space]
    public AudioSource soundPrefab;
    public int playersPoolSize = 30;

    AudioSource[] soundPlayers;

    private void Start()
    {
        soundPlayers = new AudioSource[playersPoolSize];
        for (int i = 0; i < playersPoolSize; i++)
        {
            soundPlayers[i] = Instantiate(soundPrefab, transform);
        }
    }

    private void Update()
    {
        // TODO
        /// разбей на группы по одинаковому клипу 
        /// внутри группы сортировка по расстоянию от игрока
        /// по порядку запускай из каждой следующий клип 
        foreach (var audioclip in pipe.awaitingClips)
        {
            PlaySound(audioclip.clip, audioclip.position);
        }
        pipe.awaitingClips.Clear();
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        var readySource = soundPlayers.FirstOrDefault(s => !s.isPlaying);
        if (readySource == null)
        {
            Debug.LogWarning("!No Awailable Audio Source To Play Sound!");
            return;
        }
        readySource.transform.position = position;
        readySource.clip = clip;
        readySource.Play();
    }
}
