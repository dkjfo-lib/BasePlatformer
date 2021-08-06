using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public Pipe_SoundsPlay pipe;
    private Player currentPlayer;
    [Space]
    public int maxDistance = 20;
    public int identicalClipsCount = 1;
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
        StartCoroutine(KeepPlayerActive());
    }

    private IEnumerator KeepPlayerActive()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Player.thePlayer;
        }
        while (true)
        {
            yield return new WaitUntil(() => currentPlayer == null || currentPlayer.state.IsDead);
            currentPlayer = Player.thePlayer;
        }
    }

    private void Update()
    {
        if (currentPlayer == null) return;

        // TODO
        // [x] убери слишком далекие клипы
        // [x] разбей на группы по одинаковому клипу 
        // [ ] внутри группы сортировка по расстоянию от игрока
        // [ ] по порядку запускай из каждой следующий клип 
        var clipsInRange = pipe.awaitingClips.Where(s => (s.position - currentPlayer.transform.position).sqrMagnitude < maxDistance * maxDistance);
        var identicalClips = clipsInRange.GroupBy(s => s.clipCollection);
        foreach (var sameClipsCollections in identicalClips)
        {
            int playCount = Mathf.Min(sameClipsCollections.Count(), identicalClipsCount);
            for (int i = 0; i < playCount; i++)
            {
                PlaySound(
                    sameClipsCollections.ElementAt(i).clipCollection.GetRandomClip(),
                    sameClipsCollections.ElementAt(i).position);
            }
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
