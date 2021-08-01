using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsItem", menuName = "MySounds/SoundsItem")]
public class SoundsItem : ScriptableObject
{
    public ClipsCollection onHitSounds;
    public ClipsCollection onDeathAudio;
}