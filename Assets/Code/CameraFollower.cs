using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public Vector2 offset = Vector2.zero;
    [Range(0f, 1f)] public float stickness = .5f;

    void FixedUpdate()
    {
        Vector3 offsetV3 = new Vector3(offset.x, offset.y, -10);
        transform.position = Vector3.Lerp(transform.position, target.position + offsetV3, stickness * stickness);
    }
}
