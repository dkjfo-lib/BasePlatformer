using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackStats", menuName = "My/AttackStats Melee")]
public class AttackStatsMelee : AttackStatsBase
{
    [Space]
    public Vector2 size;
    public Vector2 offset;
    [Space]
    public int damage = 1;
    public int force = 20;

    public int Damage => damage;
    public int Force => force;
    public override float DPS => damage * 1;

    public override void SetUp(Slot limb)
    {
        limb.Collider.offset = offset;
        limb.Collider.size = size;
    }
    public override void StartAttack(Slot limb)
    {

    }
    public override void OnTrigger(Slot limb, Collider2D collision)
    {
        var target = collision.GetComponent<IHittable>();
        if (limb.ShouldHit(target))
        {
            Vector2 returnForce = target.GetHit(new Hit
            {
                isRight = limb.Father.isRight,
                damage = damage,
                force = force,
                hitPosition = limb.transform.position,
                hitDirection = limb.transform.right,
            });
            limb.Father.Inertia += returnForce;
        }
    }
}
