using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Door : EventActivation
{
    public bool open = false;

    new Collider2D collider;
    SpriteRenderer spriteRenderer;

    protected override void OnStart()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Activate()
    {
        open = !open;
        collider.enabled = !open;
        spriteRenderer.enabled = !open;
    }
}
