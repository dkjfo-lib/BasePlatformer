using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveDangerDisplay : MonoBehaviour
{
    public SpawnOnEvent spawner;
    [Space]
    public Text score;

    int dps;
    private void Start()
    {
        dps = spawner.dangerPoints;
        UpdateText();
        StartCoroutine(KeepSelfUpdated());
    }

    IEnumerator KeepSelfUpdated()
    {
        while(true)
        {
            yield return new WaitUntil(() => dps != spawner.dangerPoints);
            UpdateText();
        }
    }

    void UpdateText()
    {
        dps = spawner.dangerPoints;
        score.text = dps.ToString();
    }
}
