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

    [HideInInspector] public bool detected = false;
    [HideInInspector] public GameObject[] contacts = new GameObject[0];
    [HideInInspector] public float lastOverlapTime = -1;
    void Update()
    {
        contacts = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0, layerMask).Select(s => s.gameObject).ToArray();
        detected = contacts.Length > 0;
        if (detected)
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
