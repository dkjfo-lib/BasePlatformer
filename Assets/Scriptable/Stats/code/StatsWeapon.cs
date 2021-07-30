using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "MyStats/WeaponStats")]
public class StatsWeapon : StatsBase<SoundsWeapon>
{
    public AttackStatsBase attack;
}
