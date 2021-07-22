using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharState
{
    public int health = 5;
    public bool isRight = true;
    public bool inAttack = false;
    public float timeLastAttack = -1;

    public bool IsDead => health < 1;
    public bool CanMove => !inAttack && !IsDead;
    public bool CanAttack(float attackCooldown)
    {
        bool cooldownIsOver = Time.timeSinceLevelLoad - timeLastAttack > attackCooldown;
        return !inAttack && !IsDead && cooldownIsOver;
    }
}

