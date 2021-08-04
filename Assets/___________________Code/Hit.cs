using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit
{
    public ObjectType attackerType;
    public int damage;
    public int force;
    public bool isRight;
    public Vector2 hitPosition;

    public Vector2 GetForce(Vector2 hittedObjPosition)
    {
        return isRight ?
            (hittedObjPosition - hitPosition).normalized * force :
            -(hittedObjPosition - hitPosition).normalized * force;
    }
}
