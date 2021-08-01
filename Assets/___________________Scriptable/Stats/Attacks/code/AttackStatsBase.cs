using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackStatsBase : ScriptableObject
{
    public Vector2 size;
    public Vector2 offset;
    public float closeBorder = .5f;
    public float farBorder = 2.5f;
}