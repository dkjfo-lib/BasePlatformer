using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatsPhysical", menuName = "MyStats/StatsPhysical")]
public class StatsPhysicalItem : StatsBase<SoundsPhysicalItem>
{
    public SoundsPhysicalItem Sounds => sounds;
}
