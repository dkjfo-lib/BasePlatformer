using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// World object for creature to hold items
/// </summary>
public class HittableSlot : Slot, ISlot, ICanHit, IHittable
{
    [Space]
    public bool copyCreatureStats = false;
    [Space]
    [Range(0, 5)] public float hpModifier = 1;
    [Range(0, 5)] public float armorModifier = 1;
    [Space]
    [Range(0, 1)] public float damageOnDestroyed = 0;
    public bool ignoreArmorOnDeath = true;
    [Space]
    public bool disableSpriteOnDeath = true;

    private int hp = 1;
    private int armor = 1;

    public Faction Faction => Father.Faction;

    protected override void Init()
    {
        base.Init();
        if (copyCreatureStats)
        {
            hp = Father.stats.maxHealth;
            armor = Father.stats.Armour;
        }
        else
        {
            hp = (int)(Father.stats.maxHealth * hpModifier);
            armor = (int)(Father.stats.Armour * armorModifier);
        }
    }

    public Vector2 GetHit(Hit hit)
    {
        hp -= Mathf.Max(0, hit.damage - armor);
        if (hp < 0)
        {
            OnDeath();
        }
        return Father.GetHit(hit);
    }

    public override void OnDeath()
    {
        isAlive = false;
        SpriteRenderer.enabled = !disableSpriteOnDeath;
        Collider.enabled = false;

        int destroyDamage = ignoreArmorOnDeath ?
            (int)(damageOnDestroyed * Father.state.health) + Father.stats.Armour :
            (int)(damageOnDestroyed * Father.state.health);
        Father.GetHit(new Hit(destroyDamage));
    }
}

