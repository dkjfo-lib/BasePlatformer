using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharState
{
    public int health = 10;
    public bool isRight = true;
    public bool inAttack = false;

    public bool IsDead => health < 1;
    public bool CanMove => !inAttack && !IsDead;
    public bool CanAttack => !inAttack && !IsDead;
}

