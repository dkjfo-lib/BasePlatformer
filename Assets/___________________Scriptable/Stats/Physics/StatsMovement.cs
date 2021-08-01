using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMovementStats", menuName = "MyStats/MovementStats")]
public class StatsMovement : ScriptableObject
{
    public float acceleration_H = 50;
    public float jumpSpeed = 12;
    [Range(0f, 1f)] public float speed_controll_inAir = .02f;
}