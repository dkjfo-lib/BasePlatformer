using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Player currentPlayer;
    [Space]
    public Vector2 offset = Vector2.zero;
    [Space]
    [Range(0f, 1f)] public float stickness = .5f;
    public float velocityDump = .1f;
    public float velocityMult = .1f;
    public Vector2 MaxVelocityOffset = Vector2.one;
    private Vector2 velocityOffset = Vector2.zero;

    Vector3 offsetV3 => new Vector3(offset.x, offset.y, -10);

    private void Start()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Player.thePlayer;
        }
        transform.position = currentPlayer.transform.position + offsetV3;
        StartCoroutine(KeepPlayerActive());
    }

    private IEnumerator KeepPlayerActive()
    {
        while (true)
        {
            yield return new WaitUntil(() => currentPlayer == null || currentPlayer.charState.IsDead);
            currentPlayer = Player.thePlayer;
        }
    }

    void FixedUpdate()
    {
        velocityOffset *= velocityDump;
        velocityOffset += currentPlayer.Velocity * velocityMult;
        velocityOffset = new Vector2(
            Mathf.Clamp(velocityOffset.x, -MaxVelocityOffset.x, MaxVelocityOffset.x),
            Mathf.Clamp(velocityOffset.y, -MaxVelocityOffset.y, MaxVelocityOffset.y));

        Vector3 targetPosition = currentPlayer.transform.position + (Vector3)velocityOffset + offsetV3;
        transform.position = Vector3.Lerp(transform.position, targetPosition, stickness * stickness);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, MaxVelocityOffset);
    }
}
