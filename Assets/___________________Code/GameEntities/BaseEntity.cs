using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    public bool inited { get; protected set; } = false;
    private void Start()
    {
        GetComponents();
        Init();
    }
    protected virtual void GetComponents() { }
    protected virtual void Init() { }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }
    protected virtual void OnFixedUpdate() { }

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