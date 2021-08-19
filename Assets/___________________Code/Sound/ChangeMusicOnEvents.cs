using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeMusicOnEvents : EventReceiver
{
    public ClipsCollectionToEvent[] clipsOnEvents;
    MusicPlayer music;
    protected override IEnumerable<string> ReceivedEvents => clipsOnEvents.Select(s => s.eventTag);

    protected override void GetComponents()
    {
        base.GetComponents();
        music = GetComponent<MusicPlayer>();
    }

    protected override void OnEvent(string eventTag)
    {
        music.ChangePlaylist(clipsOnEvents.First(s => s.eventTag == eventTag).clipsCollection);
    }
}

[System.Serializable]
public struct ClipsCollectionToEvent
{
    public string eventTag;
    public ClipsCollection clipsCollection;
}