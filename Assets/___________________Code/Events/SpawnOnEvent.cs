using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnOnEvent : EventReceiver
{
    public string[] SpawnEvents;
    public string[] AddDangerPointsEvents;
    public string[] ReduceDangerPointsEvents;
    public string[] EventsOnSpawnIsOver;
    protected override IEnumerable<string> ReceivedEvents => SpawnEvents.Concat(AddDangerPointsEvents).Concat(ReduceDangerPointsEvents);
    [Space]
    public int dangerPoints = 5;
    public int dangerPointsChange = 3;
    public Vector2Int dangerPointsLimits = new Vector2Int(1, 15);
    public VariantDangerPoints[] allVariants;

    SpawnerOnce[] spawners;

    protected override void GetComponents()
    {
        spawners = GetComponentsInChildren<SpawnerOnce>();
    }

    protected override void OnEvent(string eventTag)
    {
        if (SpawnEvents.Contains(eventTag))
            StartCoroutine(SpawnWave());
        if (AddDangerPointsEvents.Contains(eventTag))
        {
            if (dangerPoints + dangerPointsChange < dangerPointsLimits.y)
                dangerPoints += dangerPointsChange;
            else
                dangerPoints = dangerPointsLimits.y;
        }
        if (ReduceDangerPointsEvents.Contains(eventTag))
        {
            if (dangerPoints - dangerPointsChange > dangerPointsLimits.x)
                dangerPoints -= dangerPointsChange;
            else
                dangerPoints = dangerPointsLimits.x;
        }
    }

    IEnumerator SpawnWave()
    {
        List<VariantDangerPoints> waveSpawn = new List<VariantDangerPoints>();
        int remainingDP = dangerPoints;
        while (remainingDP > 0)
        {
            var availableVariants = allVariants.Where(s => remainingDP >= s.dangerPoints).Select(s => s).ToArray();
            var newVariant = availableVariants[Random.Range(0, availableVariants.Length)];
            remainingDP -= newVariant.dangerPoints;
            waveSpawn.Add(newVariant);
        }
        foreach (var variant in waveSpawn)
        {
            yield return new WaitForSeconds(Random.Range(.25f, 1.25f));
            SpawnVariant(variant);
        }
        foreach (var eventTag in EventsOnSpawnIsOver)
        {
            pipe_Events.SendEvent(eventTag);
        }
    }

    private void SpawnVariant(VariantDangerPoints variant)
    {
        var spwaner = spawners[Random.Range(0, spawners.Length)];
        spwaner.spawnVariants = new VariantDangerPoints[] { variant };
        spwaner.PerformSpawn();
    }
}

[System.Serializable]
public struct VariantDangerPoints
{
    public PhysicalEntityBase item;
    public int dangerPoints;
}