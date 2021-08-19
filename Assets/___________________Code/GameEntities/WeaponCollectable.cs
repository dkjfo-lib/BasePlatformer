using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO iherit PhItem
/// <summary>
/// World entity, Collectable
/// animations: idle pickup
/// </summary>
[System.Serializable]
public class WeaponCollectable
{
    public WeaponDescription description;

    public void PickUpItem(Creature creature, Limb limb)
    {
        limb.Equip(description);
    }
}
