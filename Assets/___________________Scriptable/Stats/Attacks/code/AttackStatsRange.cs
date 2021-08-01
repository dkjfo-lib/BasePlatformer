using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "My/AttackStats Range")]
public class AttackStatsRange : AttackStatsBase
{
    public Vector2 shootOffset = new Vector2(1.2f, 0);
    public Projectile projectile;

    //public override void DoAttack(Creature performer, Vector2 position, bool isRight, ObjectType attackerType)
    //{
    //    var newProjectile = GameObject.Instantiate(projectile, position + GetShootOffset(isRight), Quaternion.identity);
    //    newProjectile.attackerType = attackerType;
    //    newProjectile.facesRight = isRight;
    //}
}
