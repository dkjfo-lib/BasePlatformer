using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPhysicalStats", menuName = "My/PhysicalStats")]
public class PhysicalStats : ScriptableObject
{
    public int mass = 10;
    [Header("Horizontal")]
    public float maxSpeed_H = 600;
    public float acceleration_H = 30;
    public float minSpeedInAir_H = 6;
    [Range(0f, 1f)] public float speed_dump = .8f;
    [Header("Vertical")]
    public float gravity = 1;
    public float jumpSpeed = 10;
    [Range(0f, 1f)] public float speed_dump_inAir = .97f;
    [Range(0f, 1f)] public float speed_controll_inAir = .01f;

    public int Mass => mass;
    public float MaxSpeed_H => maxSpeed_H;
    public float Acceleration_H => acceleration_H;
    public float MinSpeedInAir_H => minSpeedInAir_H;
    public float JumpSpeed => jumpSpeed;
    public float Speed_dump_H => speed_dump;
    public float Speed_dump_inAir => speed_dump_inAir;
    public float Speed_controll_inAir => speed_controll_inAir;
}