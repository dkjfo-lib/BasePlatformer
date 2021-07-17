using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    public bool inited = false;
    private void Start()
    {
        OnStart();
    }
    protected abstract void OnStart();

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }
    protected abstract void OnFixedUpdate();

    private void OnDrawGizmos()
    {
        AddOnDrawGizmos();
    }
    protected abstract void AddOnDrawGizmos();
}
