using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackStatsBase : ScriptableObject
{
    public Vector2 offset = new Vector2(2, 0);
    public Vector2 size = new Vector2(2, .75f);
    public LayerMask characterLayerMask;
    public float cooldown = 1;

    public Vector2 Offset => offset;
    public Vector2 Size => size;
    public float Cooldown => cooldown;

    public Vector2 GetOffset(bool isRight) => isRight ? Offset : -Offset;

    public CharacterAbstract[] HasEnemies(Vector2 position, bool isRight, Faction[] enemyFactions)
    {
        var hits = Physics2D.OverlapBoxAll(position + GetOffset(isRight), Size, 0, characterLayerMask);
        var characterHits = hits.
            Select(s => s.gameObject.GetComponent<CharacterAbstract>()).
            Where(s => s != null).
            Where(s => enemyFactions.Contains(s.faction)).
            ToArray();
        return characterHits;
    }

    public abstract void DoAttack(Vector2 position, bool isRight, ObjectType attackerType);

    public virtual void OnGizmos(Vector2 position, bool isRight)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position + GetOffset(isRight), Size);
    }
}