using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharState
{
    public int health = 10;
    public bool isDead => health < 1;
    public bool isRight = true;
    public float speed_H = 0;
}
