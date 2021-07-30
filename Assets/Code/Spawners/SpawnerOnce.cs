using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOnce : MonoBehaviour
{
    public Creature[] spawnVariants;
    public Vector2 forceRangeX = new Vector2(-1000, 1000);
    public Vector2 forceRangeY = new Vector2(500, 1000);
    [Space]
    public bool onStart = false;
    public bool isOneUse = false;

    private void Start()
    {
        if (onStart)
        {
            PerformSpawn();
        }
    }

    public void PerformSpawn()
    {
        StartCoroutine(SpawnItem());
    }

    IEnumerator SpawnItem()
    {
        var chosenPrefab = spawnVariants[Random.Range(0, spawnVariants.Length)];
        var newItem = Instantiate(chosenPrefab, transform.position, Quaternion.identity);
        yield return new WaitUntil(() => newItem.inited);
        var force = new Vector2(Random.Range(forceRangeX.x, forceRangeX.y), Random.Range(forceRangeY.x, forceRangeY.y));
        newItem.Inertia += force;

        if (isOneUse)
        {
            Destroy(gameObject);
        }
    }
}
