using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttack", menuName = "My/AttackStats Range")]
public class AttackStatsRange : AttackStatsBase
{
    public Vector2 shootOffset = new Vector2(1.2f, 0);
    public Projectile projectile;

    public Vector2 GetShootOffset(bool isRight) => isRight ? shootOffset : -shootOffset;
}
