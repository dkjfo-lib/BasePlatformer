using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Number", menuName = "MyConsts/Number")]
public class Number : ScriptableObject
{
    public float number;
}

[CreateAssetMenu(fileName = "Line", menuName = "MyConsts/Line")]
public class Line : ScriptableObject
{
    public string number;
}
