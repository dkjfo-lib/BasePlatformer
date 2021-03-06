using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerKeepOneInstance : MonoBehaviour
{
    public float timeToRespawn = 3f;
    public Vector2 forceRangeX = new Vector2(-1000, 1000);
    public Vector2 forceRangeY = new Vector2(500, 1000);
    public Creature[] spawnVariants;
    public Creature instance;

    private void Start()
    {
        StartCoroutine(SpawnItem());
        StartCoroutine(KeepItemSpawned());
    }

    IEnumerator KeepItemSpawned()
    {
        while (true)
        {
            yield return new WaitUntil(() => instance == null || instance.state.IsDead);
            yield return new WaitForSeconds(timeToRespawn);
            StartCoroutine(SpawnItem());
        }
    }

    IEnumerator SpawnItem()
    {
        var chosenPrefab = spawnVariants[Random.Range(0, spawnVariants.Length)];
        instance = Instantiate(chosenPrefab, transform.position, Quaternion.identity);
        yield return new WaitUntil(() => instance.inited);
        var force = new Vector2(Random.Range(forceRangeX.x, forceRangeX.y), Random.Range(forceRangeY.x, forceRangeY.y));
        instance.Inertia += force;
    }
}
