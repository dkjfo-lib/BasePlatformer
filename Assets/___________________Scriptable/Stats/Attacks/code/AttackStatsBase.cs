using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackStatsBase : ScriptableObject
{
    public float closeBorder = .5f;
    public float farBorder = 2.5f;

    public abstract float DPS { get; }

    public abstract void SetUp(Slot limb);
    public abstract void StartAttack(Slot limb);
    public abstract void OnTrigger(Slot limb, Collider2D collision);
}