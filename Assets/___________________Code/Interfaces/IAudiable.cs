using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudiable 
{
    Pipe_SoundsPlay Pipe_SoundsPlay { get; }
}

public static class Ext_IAudiable
{
    public static void PlayAudio(this IAudiable audiable, ClipsCollection collection, Vector2 position)
    {
        if (collection == null) return;

        audiable.Pipe_SoundsPlay.awaitingClips.Add(new PlayClipData
        {
            clipCollection = collection,
            position = position
        });
    }
}