using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : PhysicalItem
{
    protected override void OnStart()
    {
        GetComponents();
    }

    protected override void OnFixedUpdate()
    {
        DampVelocity();
    }

    protected override void AddOnDrawGizmos() { }
}