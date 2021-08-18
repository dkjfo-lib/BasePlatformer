using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableType : ScriptableObject
{
    public abstract void OnPickUp(InetractionParams values);
}
