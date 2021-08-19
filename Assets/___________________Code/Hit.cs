using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit
{
    public static readonly Hit SelfDestroy = new Hit
    {
        damage = 9999,
        force = 0,
        hitDirection = Vector2.down,
        hitPosition = Vector2.zero,
        isRight = true
    };

    //public ObjectType attackerType;
    public int damage;
    public int force;
    public bool isRight;
    public Vector2 hitPosition;
    public Vector2 hitDirection;

    public Vector2 GetForce()
    {
        return GetHitDirection().normalized * force;
    }
    public Vector2 GetHitDirection()
    {
        return isRight ? hitDirection : -hitDirection;
    }
}
