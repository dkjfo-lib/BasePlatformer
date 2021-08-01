using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController controller;

    public Sound soundPrefab;

    private void Start()
    {
        controller = this;
    }

    public static void PlaySound(AudioClip sound, Vector2 position = new Vector2(), Transform parent = null)
    {
        Instantiate(controller.soundPrefab, position, Quaternion.identity, parent).PlaySound(sound);
    }
}
