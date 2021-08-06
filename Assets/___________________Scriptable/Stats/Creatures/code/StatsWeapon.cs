using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// animations: idle attack
/// </summary>
[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "MyStats/WeaponStats")]
public class StatsWeapon : StatsBase<SoundsWeapon>
{
    public Bool isFriendlyDamageOn;
    public Bool isSelfDamageOn;
    [Space]
    public AttackStatsBase attack;
}
