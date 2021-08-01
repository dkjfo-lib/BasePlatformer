using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit
{
    public ObjectType attackerType;
    public int damage;
    public int force;
    public bool isRight;

    public float GetForce => isRight ? force : -force;
}