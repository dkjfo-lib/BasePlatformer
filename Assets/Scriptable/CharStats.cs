using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "My/CharacterStats")]
public class CharStats : ScriptableObject
{
    public int maxHealth = 10;

    public int MaxHealth => maxHealth;
}