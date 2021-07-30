using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateItem
{
    public int health = 5;
    public bool isRight = true;

    public bool IsDead => health < 1;

    public void Init(int maxHP)
    {
        health = maxHP;
    }
}

[System.Serializable]
public class StateCharacter : StateItem
{
    [HideInInspector] public bool inAttack = false;
    [HideInInspector] public float timeLastAttack = -1;
    public FactionAlignment alignment;

    public bool CanMove => !inAttack && !IsDead;
    public bool CanAttack(float attackCooldown)
    {
        bool cooldownIsOver = Time.timeSinceLevelLoad - timeLastAttack > attackCooldown;
        return !inAttack && !IsDead && cooldownIsOver;
    }
}

[System.Serializable]
public class StateWeapon : StateItem
{
    public float timeLastAttack = -1;

    public bool CanAttack(float attackCooldown)
    {
        bool cooldownIsOver = Time.timeSinceLevelLoad - timeLastAttack > attackCooldown;
        return !IsDead && cooldownIsOver;
    }
}

