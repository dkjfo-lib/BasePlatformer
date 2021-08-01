using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPhysicsStats", menuName = "MyStats/PhysicsStats")]
public class StatsPhysics : ScriptableObject
{
    public int mass = 10;
    [Range(0, 1)] public float toughness = .25f;
    public Number gravity;
    [Space]
    public float maxSpeed_H = 20;
    public float minSpeedInAir_H = 6;
    [Space]
    [Range(0f, 1f)] public float speed_dump_inAir = .97f;
    [Range(0f, 1f)] public float speed_dump = .85f;
}