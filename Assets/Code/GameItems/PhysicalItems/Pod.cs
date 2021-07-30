using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : PhysicalItem<StatsPhysicalItem, SoundsItem, StateItem>
{
    public SpawnerOnce[] crewSpawners;

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (OnGround)
        {
            Open();
        }
    }

    public void Open()
    {
        Anim_SetTrigger("open");
    }

    public void Spawn()
    {
        foreach (var spawn in crewSpawners)
        {
            spawn.PerformSpawn();
        }
        state.health = -1;
    }
}
