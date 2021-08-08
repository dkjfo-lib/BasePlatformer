using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioStats<TSounds> where TSounds : SoundsPhysicalItem
{
    public TSounds Sounds { get; }
}

public abstract class StatsBase<TSounds> : ScriptableObject 
    where TSounds : SoundsPhysicalItem
{
    public ObjectType entityType = ObjectType.UNDEFINED;
    public int maxHealth = 10;
    public int Armour = 0;
    [Space]
    public StatsPhysics physics;
    public RuntimeAnimatorController animator;
    public TSounds sounds;
}
