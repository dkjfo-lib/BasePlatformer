using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizer : MonoBehaviour
{
    Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();

    }

    void Update()
    {

    }
}
