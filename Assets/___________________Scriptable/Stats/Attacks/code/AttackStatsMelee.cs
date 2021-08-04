using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackStats", menuName = "My/AttackStats Melee")]
public class AttackStatsMelee : AttackStatsBase
{
    public int damage = 1;
    public int force = 20;

    public int Damage => damage;
    public int Force => force;
}
