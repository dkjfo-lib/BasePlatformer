using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "My/AttackStats Range")]
public class AttackStatsRange : AttackStatsBase
{
    public Projectile projectile;
    public override float DPS => projectile.stats.attack.DPS;

    public override void SetUp(Slot limb)
    {

    }
    public override void StartAttack(Slot limb)
    {
        var newProjectile = Instantiate(projectile, limb.FirePoint.position, limb.FirePoint.transform.rotation);
        newProjectile.state.isRight = limb.Father.isRight;
        newProjectile.state.alignment = limb.Father.state.alignment;
    }
    public override void OnTrigger(Slot limb, Collider2D collision)
    {

    }
}
