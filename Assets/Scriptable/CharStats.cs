using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "My/CharacterStats")]
public class CharStats : ScriptableObject
{
    public CharStatsMovement movement;
    public int maxHealth = 10;

    public CharStatsMovement Movement => movement;
    public int MaxHealth => maxHealth;
}

[System.Serializable]
public class CharStatsMovement
{
    [Header("Horizontal")]
    public float maxSpeed_H = 600;
    public float acceleration_H = 30;
    public float minSpeedInAir_H = 6;
    [Range(0f, 1f)] public float speed_dump = .8f;
    [Header("Vertical")]
    public float jumpSpeed = 10;
    [Range(0f, 1f)] public float speed_dump_inAir = .97f;
    [Range(0f, 1f)] public float speed_controll_inAir = .01f;

    public float MaxSpeed_H => maxSpeed_H;
    public float Acceleration_H => acceleration_H;
    public float MinSpeedInAir_H => minSpeedInAir_H;
    public float JumpSpeed => jumpSpeed;
    public float Speed_dump_H => speed_dump;
    public float Speed_dump_inAir => speed_dump_inAir;
    public float Speed_controll_inAir => speed_controll_inAir;
}