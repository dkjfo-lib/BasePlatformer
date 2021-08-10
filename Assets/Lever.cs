using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInetractable
{
    public Pipe_Events pipe_Events;
    [Space]
    public string activationTag = "404";

    private void Start()
    {
        if (activationTag == "404") Debug.LogWarning("Tag: 404", gameObject);
    }

    public void Inetract(InetractionParams values)
    {
        pipe_Events.AddEvent(activationTag);
    }
}
