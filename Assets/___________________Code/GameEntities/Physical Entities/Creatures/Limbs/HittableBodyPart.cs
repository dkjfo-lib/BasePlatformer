using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableBodyPart : MonoBehaviour, ISlot, IHittable
{
    public bool copyCreatureStats = false;
    [Space]
    [Range(0, 5)] public float hpModifier = 1;
    [Range(0, 5)] public float armorModifier = 1;
    [Space]
    [Range(0, 1)] public float damageOnDestroyed = 0;
    public bool ignoreArmorOnDeath = true;
    [Space]
    public bool disableSpriteOnGettinKilled = true;
    public bool disableSpriteOnHostDeath = true;

    private int hp = 1;
    private int armor = 1;

    public Faction Faction => Father.Faction;

    Creature Father;
    SpriteRenderer SpriteRenderer;
    Collider2D Collider;

    private void Start()
    {
        Father = GetComponentInParent<Creature>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
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
            GetKilled();
        }
        return Father.GetHit(hit);
    }

    private void GetKilled()
    {
        int destroyDamage = ignoreArmorOnDeath ?
            (int)(damageOnDestroyed * Father.state.health) + Father.stats.Armour :
            (int)(damageOnDestroyed * Father.state.health);
        Father.GetHit(new Hit(destroyDamage));
        SpriteRenderer.enabled = !disableSpriteOnGettinKilled;
    }

    public void OnHostDeath()
    {
        SpriteRenderer.enabled = !disableSpriteOnHostDeath;
    }
}
