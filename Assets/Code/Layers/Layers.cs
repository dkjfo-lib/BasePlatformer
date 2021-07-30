using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour
{
    public static LayerMask Ground { get; }
    public static LayerMask Items { get; }
    public static LayerMask Hittable { get; }

    static Layers()
    {
        Ground = LayerMask.GetMask("Ground");
        Items = LayerMask.GetMask("Items");
        Hittable = LayerMask.GetMask("Characters", "Items");
    }
}
