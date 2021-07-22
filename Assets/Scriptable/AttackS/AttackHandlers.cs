using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IAttackHandler
{
    CharacterAbstract[] HasEnemies(Vector2 position, bool isRight);

    void DoAttack(Vector2 position, bool isRight, ObjectType attackerType);
}

public abstract class AttackHandlerBase<T> : IAttackHandler where T : AttackStatsBase
{
    public T stats { get; set; }

    public CharacterAbstract[] HasEnemies(Vector2 position, bool isRight)
    {
        var hits = Physics2D.OverlapBoxAll(position + stats.GetOffset(isRight), stats.size, 0, stats.triggerLayerMask);
        var characterHits = hits.Select(s => s.gameObject.GetComponent<CharacterAbstract>()).Where(s => s != null).ToArray();
        return characterHits;
    }

    public abstract void DoAttack(Vector2 position, bool isRight, ObjectType attackerType);

}

[System.Serializable]
public class AttackHandlerMelee : AttackHandlerBase<AttackStatsMelee>
{
    public AttackHandlerMelee(AttackStatsMelee stats)
    {
        this.stats = stats;
    }

    public CharacterAbstract[] CastAttack(Vector2 position, bool isRight)
    {
        var hits = Physics2D.OverlapBoxAll(position + stats.GetOffset(isRight), stats.size, 0, stats.attackLayerMask);
        var characterHits = hits.Select(s => s.gameObject.GetComponent<CharacterAbstract>()).Where(s => s != null).ToArray();
        return characterHits;
    }

    public override void DoAttack(Vector2 position, bool isRight, ObjectType attackerType)
    {
        var hits = CastAttack(position, isRight);
        foreach (var hit in hits)
        {
            hit.GetHit(new Hit
            {
                attackerType = attackerType,
                damage = stats.damage,
                force = stats.force,
                position = position
            });
        }

    }
}

[System.Serializable]
public class AttackHandlerRange : AttackHandlerBase<AttackStatsRange>
{
    public AttackHandlerRange(AttackStatsRange stats)
    {
        this.stats = stats;
    }

    public override void DoAttack(Vector2 position, bool isRight, ObjectType attackerType)
    {
        var newProjectile = GameObject.Instantiate(stats.projectile, position + stats.GetShootOffset(isRight), Quaternion.identity);
        newProjectile.attackerType = attackerType;
        newProjectile.isRight = isRight;
    }
}

public static class AttackHandlerHelper
{
    public static IAttackHandler GetAttackHandler(AttackStatsBase stats)
    {
        IAttackHandler attack = null;
        if (stats is AttackStatsMelee)
            attack = new AttackHandlerMelee(stats as AttackStatsMelee);
        if (stats is AttackStatsRange)
            attack = new AttackHandlerRange(stats as AttackStatsRange);
        return attack;
    }

    public static void OnGizmos(AttackStatsBase stats, Vector2 position, bool isRight)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position + stats.GetOffset(isRight), stats.size);
        if (stats is AttackStatsRange)
        {
            Gizmos.DrawWireCube(position + (stats as AttackStatsRange).GetShootOffset(isRight), Vector3.one * .25f);
        }
    }
}