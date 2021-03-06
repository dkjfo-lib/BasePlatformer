using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Player currentPlayer;
    [Space]
    public Vector2 offset = Vector2.zero;
    public float directionOffset = 2;
    public float zoom = 2;
    [Space]
    [Range(0f, 1f)] public float stickness = .5f;
    [Range(1f, 2f)] public float screenBorder = 1.2f;

    public bool isRight => currentPlayer.isRight;

    Vector3 Offset => new Vector3(offset.x, offset.y, -10);

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
            yield return new WaitUntil(() => currentPlayer == null || currentPlayer.state.IsDead);
            currentPlayer = Player.thePlayer;
        }
    }

    void FixedUpdate()
    {
        if (currentPlayer == null) return;

        var pX = Mathf.Clamp((Input.mousePosition.x / Screen.width * 2 - 1) * screenBorder, -1, 1);
        var pY = Mathf.Clamp((Input.mousePosition.y / Screen.height * 2 - 1) * screenBorder, -1, 1);

        var directionOffset = new Vector3(pX, pY, 0) * this.directionOffset;
        Vector3 targetPosition = currentPlayer.transform.position + Offset + directionOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, stickness * stickness);
    }
}
