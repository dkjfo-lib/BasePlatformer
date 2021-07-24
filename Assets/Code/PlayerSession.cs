using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    static PlayerSession session;

    private void Awake()
    {
        if (session == null)
        {
            session = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
