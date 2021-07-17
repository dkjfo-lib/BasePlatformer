using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    public bool inited { get; protected set; } = false;
    private void Start()
    {
        GetComponents();
        Init();
    }
    protected abstract void GetComponents();
    protected abstract void Init();

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }
    protected abstract void OnFixedUpdate();

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        AddOnDrawGizmos();
    }
    protected virtual void AddOnDrawGizmos() { }
}
