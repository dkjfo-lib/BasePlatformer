using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class Limb : MonoBehaviour
{
    public ItemDescription item;
}

/// <summary>
/// Description of weapon and it's state
/// </summary>
[System.Serializable]
public class WeaponDescription : ItemDescription
{
    public StatsWeapon stats;
    public StateWeapon state;

    public override void UseItem()
    {
        // play anim attack
        state.timeLastAttack = Time.timeSinceLevelLoad;
    }

    // calls in animation
    private void CastAttack()
    {
        // phisics.overlap, choose targets
    }
}

// TODO iherit PhItem
/// <summary>
/// World entity, Collectable
/// </summary>
[System.Serializable]
public class WeaponItem : PickableItem<WeaponDescription> { }

/// <summary>
/// Base class for world entities of collectable items
/// </summary>
[System.Serializable]
public abstract class PickableItem<TItemDescription>
    where TItemDescription : ItemDescription
{
    public TItemDescription item;

    public void PickUpItem(Creature creature, Limb limb)
    {
        limb.item = item;
    }
}

/// <summary>
/// Base class for collectable Item description
/// </summary>
[System.Serializable]
public abstract class ItemDescription
{
    public abstract void UseItem();
}