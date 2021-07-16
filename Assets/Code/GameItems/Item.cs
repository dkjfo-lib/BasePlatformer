using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : GraphicalItem
{
    protected override void OnStart()
    {
        GetComponents();
    }

    protected override void OnFixedUpdate() { }

    protected override void AddOnDrawGizmos() { }
}
