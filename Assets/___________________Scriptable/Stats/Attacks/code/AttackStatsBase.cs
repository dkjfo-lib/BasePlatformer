using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackStatsBase : ScriptableObject
{
    public float closeBorder = .5f;
    public float farBorder = 2.5f;

    public abstract void SetUp(Limb limb);
    public abstract void StartAttack(Limb limb);
    public abstract void OnTrigger(Limb limb, Collider2D collision);
}