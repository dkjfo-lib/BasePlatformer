using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackStatsBase : ScriptableObject
{
    public Vector2 offset = new Vector2(2, 0);
    public Vector2 size = new Vector2(2, .75f);
    public LayerMask triggerLayerMask;
    public LayerMask attackLayerMask;
    public float cooldown = 1;

    public Vector2 GetOffset(bool isRight) => isRight ? offset : -offset;
}

[CreateAssetMenu(fileName = "NewAttackStats", menuName = "My/AttackStats Melee")]
public class AttackStatsMelee : AttackStatsBase
{
    public int damage = 1; 
    public int force = 1000;
}
