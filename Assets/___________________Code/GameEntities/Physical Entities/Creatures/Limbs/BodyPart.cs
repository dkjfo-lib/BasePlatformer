using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour, ISlot
{
    SpriteRenderer SpriteRenderer { get; set; }

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnHostDeath()
    {
        SpriteRenderer.enabled = false;
    }
}
