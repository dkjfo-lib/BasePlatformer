using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatsPhysical", menuName = "MyStats/StatsPhysical")]
public class StatsPhysicalItem : StatsBase<SoundsItem>
{
    public SoundsItem Sounds => sounds;
}
