using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMyRect
{
    Vector2 Size { get; }
    Vector2 GetOffset(bool isRight);
}

[System.Serializable]
public class MyRect : IMyRect
{
    public Vector2 size = Vector2.one;
    public Vector2 offset = Vector2.zero;

    public Vector2 Size => size;
    public Vector2 GetOffset(bool isRight) => isRight ? offset : new Vector2(-offset.x, offset.y);
}

[System.Serializable]
public class MyCastRect : IMyRect
{
    public MyRect myRect;
    public LayerMask layerMask;

    public Vector2 Size => myRect.Size;
    public Vector2 GetOffset(bool isRight) => myRect.GetOffset(isRight);

    public Collider2D[] Cast(Vector2 position, bool isRight) => Physics2D.OverlapBoxAll(position + GetOffset(isRight), myRect.size, 0, layerMask);
}
