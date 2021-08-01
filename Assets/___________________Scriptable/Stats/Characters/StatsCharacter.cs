using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "MyStats/CharacterStats")]
public class StatsCharacter : StatsBase<SoundsCharacter>
{
    public string attackAnimationName;
    public StatsMovement statsMovement;
}
