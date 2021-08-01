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
public abstract class MyCastRect : MyRect
{
    public abstract LayerMask LayerMask { get; }
    public abstract Color GizmosColor { get; }

    public Collider2D[] Cast(Vector2 position, bool isRight)
    {
        return Physics2D.OverlapBoxAll(position + GetOffset(isRight), Size, 0, LayerMask);
    }

    public void OnGizmos(Vector2 position, bool isRight)
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawWireCube(position + GetOffset(isRight), Size);
    }
}
