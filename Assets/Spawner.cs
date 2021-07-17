using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float timeBetween = 3f;
    public Vector2 forceRangeX = new Vector2(-1000, 1000);
    public Vector2 forceRangeY = new Vector2(500, 1000);
    public PhysicalItem item;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetween);
            item = Instantiate(item, transform.position, Quaternion.identity);
            var force = new Vector2(Random.Range(forceRangeX.x, forceRangeX.y), Random.Range(forceRangeY.x, forceRangeY.y));
            yield return new WaitUntil(() => item.inited);
            item.AddForce(force);
        }
    }
}
