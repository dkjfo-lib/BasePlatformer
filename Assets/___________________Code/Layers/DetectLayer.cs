using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DetectLayer<T> : MyCastRect where T : MonoBehaviour
{
    [HideInInspector] public Collider2D[] contacts = new Collider2D[0];
    [HideInInspector] public float lastOverlapTime = -1;
    public bool Detected => contacts.Length > 0;
    public IEnumerable<T> Contacts => contacts.
        Select(s => s.GetComponent<T>()).Where(s => s != null);

    public void UpdateDetector(Vector2 position, bool isRight)
    {
        contacts = Cast(position, isRight);
        if (Detected)
        {
            lastOverlapTime = Time.timeSinceLevelLoad;
        }
    }
}

[System.Serializable]
public class DetectGround : DetectLayer<MonoBehaviour>
{
    public bool usePlatforms = true;
    public override LayerMask LayerMask => usePlatforms ? Layers.GroundAndPlatforms : Layers.Ground;
    public override Color GizmosColor => Color.yellow;
}

[System.Serializable]
public class DetectHittable : DetectLayer<Creature>
{
    public override LayerMask LayerMask => Layers.Hittable;
    public override Color GizmosColor => Color.magenta;
}

[System.Serializable]
public class DetectActive : DetectLayer<Creature>
{
    public override LayerMask LayerMask => Layers.Hittable;
    public override Color GizmosColor => Color.green;
}