using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOnce : MonoBehaviour
{
    public VariantDangerPoints[] spawnVariants;
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
        var chosenVariant = spawnVariants[Random.Range(0, spawnVariants.Length)];
        if (chosenVariant.Bot == null)
        {
            var newItem = Instantiate(chosenVariant.Pod, transform.position, Quaternion.identity);
            var force = new Vector2(Random.Range(forceRangeX.x, forceRangeX.y), Random.Range(forceRangeY.x, forceRangeY.y));
            newItem.state.isRight = force.x > 0;
            yield return new WaitUntil(() => newItem.inited);
            newItem.Inertia += force;
        }
        else
        {
            var newItem = Instantiate(chosenVariant.Bot, transform.position, Quaternion.identity);
            var force = new Vector2(Random.Range(forceRangeX.x, forceRangeX.y), Random.Range(forceRangeY.x, forceRangeY.y));
            newItem.state.isRight = force.x > 0;
            yield return new WaitUntil(() => newItem.inited);
            newItem.Inertia += force;
        }

        if (isOneUse)
        {
            Destroy(gameObject);
        }
    }
}
