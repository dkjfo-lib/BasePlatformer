using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public static PlayerRespawn playerSpawner;

    private void Awake()
    {
        if (playerSpawner != null)
        {
            Debug.LogError("PlayerRespawn destroyed as its a dupe! One PlayerRespawn on scene only!");
            Destroy(gameObject);
        }
        playerSpawner = this;
    }
}
