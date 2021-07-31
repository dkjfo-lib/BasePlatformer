using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateItem
{
    public int health = 5;
    public bool isRight = true;
    public FactionAlignment alignment;

    public bool IsDead => health < 1;

    public void Init(int maxHP)
    {
        health = maxHP;
    }
}

[System.Serializable]
public class StateCharacter : StateItem
{
    public bool CanAttack()
    {
        return !IsDead;
    }
}

[System.Serializable]
public class StateWeapon : StateItem
{
    public bool CanAttack()
    {
        return !IsDead;
    }
}

