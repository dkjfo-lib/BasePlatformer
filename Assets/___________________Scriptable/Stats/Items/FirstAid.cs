using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeHeal", menuName = "MyItemTypes/Heal")]
public class FirstAid : ConsumableType
{
    public override void OnPickUp(InetractionParams values)
    {
        values.character.state.health = values.character.stats.maxHealth;
        values.character.UpdateGUI(true);
    }
}
