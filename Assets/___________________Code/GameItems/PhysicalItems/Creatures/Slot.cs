using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Slot : MonoBehaviour, ISlot
{
    SpriteRenderer SpriteRenderer { get; set; }

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDeath()
    {
        SpriteRenderer.enabled = false;
    }
}

public interface ISlot
{
    void OnDeath();
}
