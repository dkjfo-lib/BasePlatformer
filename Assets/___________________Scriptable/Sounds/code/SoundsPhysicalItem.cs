using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundsPhysicalItem 
{
    public ClipsCollection idleSounds;
    public ClipsCollection onHitSounds;
    public ClipsCollection onDeathAudio;
}

[System.Serializable]
public class SoundsCharacter : SoundsPhysicalItem
{
    public ClipsCollection onHitScreams;
    public ClipsCollection onAttackScreams;
}

[System.Serializable]
public class SoundsWeapon : SoundsPhysicalItem
{
    public ClipsCollection onAttackSounds;
}