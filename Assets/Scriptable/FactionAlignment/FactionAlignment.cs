using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFactionAlignment", menuName = "My/FactionAlignment")]
public class FactionAlignment : ScriptableObject
{
    public Faction faction;
    public Faction[] enemyFactions;
}
