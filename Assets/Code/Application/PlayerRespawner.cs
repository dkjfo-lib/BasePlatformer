using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    public Player playerPrefab;
    public float respawnSeconds = 3f;

    private void Start()
    {
        StartCoroutine(KeepPlayerActive());
    }

    private IEnumerator KeepPlayerActive()
    {
        while (true)
        {
            yield return new WaitUntil(() => PlayerNeedsRespawn());
            yield return new WaitForSeconds(respawnSeconds);
            if (PlayerNeedsRespawn())
            {
                RespawnPlayer();
            }
        }
    }

    bool PlayerNeedsRespawn()
    {
        return Player.thePlayer == null || Player.thePlayer.state.IsDead;
    }

    private void RespawnPlayer()
    {
        Instantiate(playerPrefab, PlayerRespawn.playerSpawner.transform.position, Quaternion.identity);
    }
}
