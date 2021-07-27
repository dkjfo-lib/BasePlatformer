using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AttackStatsBase : ScriptableObject, IMyRect
{
    public MyCastRect attackRect;
    public float cooldown = 1;

    public Vector2 Size => attackRect.Size;
    public Vector2 GetOffset(bool isRight) => attackRect.GetOffset(isRight);
    public float Cooldown => cooldown;

    public CharacterAbstract[] HasEnemies(Vector2 position, bool isRight, Faction[] enemyFactions)
    {
        var hits = attackRect.Cast(position, isRight);
        var characterHits = hits.
            Select(s => s.gameObject.GetComponent<CharacterAbstract>()).
            Where(s => s != null).
            Where(s => enemyFactions.Contains(s.faction)).
            ToArray();
        return characterHits;
    }

    public abstract void DoAttack(CharacterAbstract performer, Vector2 position, bool isRight, ObjectType attackerType);

    public virtual void OnGizmos(Vector2 position, bool isRight)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position + GetOffset(isRight), Size);
    }
}