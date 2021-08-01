using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsCharacter", menuName = "MySounds/SoundsCharacter")]
public class SoundsCharacter : SoundsItem
{
    public ClipsCollection onHitScreams;
    public ClipsCollection onAttackScreams;
}