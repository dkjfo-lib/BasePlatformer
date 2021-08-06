using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "My/AttackStats Range")]
public class AttackStatsRange : AttackStatsBase
{
    public Projectile projectile;

    public override void SetUp(Limb limb)
    {

    }
    public override void StartAttack(Limb limb)
    {
        var newProjectile = Instantiate(projectile, limb.FirePoint.position, limb.FirePoint.transform.rotation);
        newProjectile.state.isRight = limb.Father.isRight;
        newProjectile.state.alignment = limb.Father.state.alignment;
    }
    public override void OnTrigger(Limb limb, Collider2D collision)
    {

    }
}
