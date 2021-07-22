using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectLayer : MonoBehaviour
{
    public Vector2 size = Vector2.one;
    public Vector2 offset = Vector2.zero;
    public LayerMask layerMask;
    public Color gizmosColor = Color.yellow;

    [HideInInspector] public Collider2D[] contacts = new Collider2D[0];
    [HideInInspector] public bool Detected => contacts.Length > 0;
    [HideInInspector] public T[] GetContacts<T>(System.Func<Collider2D, T> func) => contacts.Select(s => func(s)).ToArray();
    [HideInInspector] public float lastOverlapTime = -1;
    void Update()
    {
        contacts = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0, layerMask);
        if (Detected)
        {
            lastOverlapTime = Time.timeSinceLevelLoad;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube((Vector2)transform.position + offset, size);
    }

    public void Flip_H()
    {
        offset = new Vector2(-offset.x, offset.y);
    }
}
