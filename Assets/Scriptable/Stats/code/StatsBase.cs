using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioStats<TSounds> where TSounds : SoundsItem
{
    public TSounds Sounds { get; }
}

public abstract class StatsBase<TSounds> : ScriptableObject 
    where TSounds : SoundsItem
{
    public ObjectType entityType = ObjectType.UNDEFINED;
    public int maxHealth = 10;
    [Space]
    public StatsPhysics physics;
    public RuntimeAnimatorController animator;
    public string attackAnimationName;
    public TSounds sounds;
}
