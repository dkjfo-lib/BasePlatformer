using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Player currentPlayer;
    [Space]
    public Vector2 offset = Vector2.zero;
    public float directionOffset = 2;
    [Space]
    [Range(0f, 1f)] public float stickness = .5f;
    public bool isRight => currentPlayer.isRight;

    Vector3 Offset => new Vector3(offset.x, offset.y, -10);
    Vector3 DirectionOffset => isRight ? Vector3.right * directionOffset : -Vector3.right * directionOffset;

    private void Start()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Player.thePlayer;
        }
        transform.position = currentPlayer.transform.position + Offset;
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
        Vector3 targetPosition = currentPlayer.transform.position + Offset + DirectionOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, stickness * stickness);
    }
}
