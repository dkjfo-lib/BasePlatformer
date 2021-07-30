using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackStats", menuName = "My/AttackStats Melee")]
public class AttackStatsMelee : AttackStatsBase
{
    public int damage = 1;
    public int force = 1000;

    public int Damage => damage;
    public int Force => force;

    public Creature[] CastAttack(Creature performer, Vector2 position, bool isRight)
    {
        var hits = attackRect.Cast(position, isRight);
        var characterHits = hits.
            Select(s => s.gameObject.GetComponent<Creature>()).
            Where(s => s != null && s != performer).ToArray();
        return characterHits;
    }

    public override void DoAttack(Creature performer, Vector2 position, bool isRight, ObjectType attackerType)
    {
        var hits = CastAttack(performer, position, isRight);
        foreach (var hit in hits)
        {
            hit.GetHit(new Hit
            {
                attackerType = attackerType,
                isRight = isRight,
                damage = Damage,
                force = Force,
            });
        }
    }
}
