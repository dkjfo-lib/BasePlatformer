using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : PhysicalItem<StatsPhysicalItem, SoundsPhysicalItem, StateItem>
{
    protected SpawnerOnce[] CrewSpawners { get; set; }

    protected override void GetComponents()
    {
        base.GetComponents();
        CrewSpawners = GetComponentsInChildren<SpawnerOnce>();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (OnGround && !state.IsDead)
        {
            Open();
        }
    }

    public void Open()
    {
        Anim_SetTrigger("open");
        // kills the pod triggering landing sounds and particles
        // TODO: if somehow killed before landing => BUG
        GetHit(new Hit
        {
            attackerType = ObjectType.UNDEFINED,
            damage = stats.maxHealth,
            force = 0,
            isRight = true
        });
    }

    /// <summary>
    /// Use In Animation
    /// </summary>
    public void Spawn()
    {
        foreach (var spawn in CrewSpawners)
        {
            spawn.PerformSpawn();
        }
        state.health = -1;
    }
}
