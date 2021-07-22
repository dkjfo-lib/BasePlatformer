using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackStats", menuName = "My/AttackStats Melee")]
public class AttackStatsMelee : AttackStatsBase
{
    public int damage = 1;
    public int force = 1000;
    public LayerMask attackLayerMask;

    public int Damage => damage;
    public int Force => force;
    public LayerMask AttackLayerMask => attackLayerMask;

    public CharacterAbstract[] CastAttack(Vector2 position, bool isRight)
    {
        var hits = Physics2D.OverlapBoxAll(position + GetOffset(isRight), Size, 0, AttackLayerMask);
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
                position = position,
                damage = Damage,
                force = Force,
            });
        }
    }
}
