using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public PhysicalItem target;
    public Vector2 offset = Vector2.zero;
    [Range(0f, 1f)] public float stickness = .5f;
    public float velocityDump = .1f;
    public float velocityMult = .1f;
    public Vector2 MaxVelocityOffset = Vector2.one;
    private Vector2 velocityOffset = Vector2.zero;

    Vector3 offsetV3 => new Vector3(offset.x, offset.y, -10);

    private void Start()
    {
        transform.position = target.transform.position + offsetV3;
    }

    void FixedUpdate()
    {
        velocityOffset *= velocityDump;
        velocityOffset += target.Velocity * velocityMult;
        velocityOffset = new Vector2(
            Mathf.Clamp(velocityOffset.x, -MaxVelocityOffset.x, MaxVelocityOffset.x),
            Mathf.Clamp(velocityOffset.y, -MaxVelocityOffset.y, MaxVelocityOffset.y));
        Vector3 targetPosition = target.transform.position + (Vector3)velocityOffset + offsetV3;
        transform.position = Vector3.Lerp(transform.position, targetPosition, stickness * stickness);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, MaxVelocityOffset);
    }
}
