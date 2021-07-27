using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DetectLayer : IMyRect
{
    public MyCastRect detectorRect;
    public Color gizmosColor = Color.yellow;

    [HideInInspector] public Collider2D[] contacts = new Collider2D[0];
    [HideInInspector] public bool Detected => contacts.Length > 0;
    [HideInInspector] public T[] GetContacts<T>(System.Func<Collider2D, T> func) => contacts.Select(s => func(s)).ToArray();
    [HideInInspector] public float lastOverlapTime = -1;

    public Vector2 Size => detectorRect.Size;
    public Vector2 GetOffset(bool isRight) => detectorRect.GetOffset(isRight);

    public void UpdateDetector(Vector2 position, bool isRight)
    {
        contacts = detectorRect.Cast(position, isRight);
        if (Detected)
        {
            lastOverlapTime = Time.timeSinceLevelLoad;
        }
    }

    public void OnGizmos(Vector2 position, bool isRight)
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(position + GetOffset(isRight), Size);
    }
}
