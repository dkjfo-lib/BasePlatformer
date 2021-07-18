using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player currentPlayer;
    public Player playerPrefab;
    public CameraFollower cameraFollower;
    public Respawn respawnPoint;
    public float respawnSeconds = 3f;

    private void Start()
    {
        StartCoroutine(KeepPlayerActive());
    }

    private IEnumerator KeepPlayerActive()
    {
        while (true)
        {
            yield return new WaitUntil(() => currentPlayer == null || currentPlayer.charState.IsDead);
            yield return new WaitForSeconds(respawnSeconds);
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, respawnPoint.transform.position, Quaternion.identity);
        cameraFollower.target = currentPlayer.transform;
    }
}
