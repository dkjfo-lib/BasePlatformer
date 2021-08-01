using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGUI : MonoBehaviour
{
    public Canvas canvas;
    public Slider healthSlider;

    public void Init(int maxHealth, StateItem charState)
    {
        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;
        SetHP(charState.health);
    }

    public void UpdateUI(StateItem charState, bool isDisplay)
    {
        SetHP(charState.health);
        DisplayGUI(isDisplay);
    }

    void SetHP(int hp)
    {
        if (healthSlider == null) return;
        healthSlider.value = hp;
    }

    void DisplayGUI(bool isDisplay)
    {
        if (canvas.gameObject.activeSelf != isDisplay)
            canvas.gameObject.SetActive(isDisplay);
    }
}
