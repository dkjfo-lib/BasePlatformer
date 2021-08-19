using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : PhysicalEntity<StatsPhysicalItem, SoundsPhysicalItem, StateItem>, IInetractable
{
    [Space]
    public ConsumableType consumableItem;

    public void Inetract(InetractionParams values)
    {
        consumableItem.OnPickUp(values);
        GetHit(Hit.SelfDestroy);
    }
}