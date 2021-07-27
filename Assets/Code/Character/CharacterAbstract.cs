using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterGUI))]
public class CharacterAbstract : PhysicalItem
{
    public CharStats charStats;
    public CharState charState;
    public AttackStatsBase attackStats;
    public ObjectType characterType = ObjectType.UNDEFINED;
    [Space]
    public ClipsCollection hitSounds;
    public ClipsCollection hitScreams;
    public ClipsCollection attackScreams;
    public ClipsCollection attackSound;
    public ClipsCollection deathScreams;
    [Space]
    public Faction faction;
    public Faction corpseFaction;
    public string corpseLayer = "Items";

    public override bool isRight => charState.isRight;
    CharacterGUI characterGUI;

    protected override void GetComponents()
    {
        base.GetComponents();
        characterGUI = GetComponent<CharacterGUI>();
    }

    protected override void Init()
    {
        base.Init();
        charState.health = charStats.MaxHealth;
        if (!charState.isRight)
        {
            Flip_H();
            charState.isRight = !charState.isRight;
        }
        characterGUI.Init(charStats, charState);
    }

    protected void DoAttack(string attackName)
    {
        if (charState.CanAttack(attackStats.cooldown))
        {
            charState.timeLastAttack = Time.timeSinceLevelLoad;
            charState.inAttack = true;
            Anim_SetTrigger("attack");
            StartCoroutine(WaitWhileAttack(attackName));
            PlaySound(attackScreams);
        }
    }
    public void CastAttack()
    {
        attackStats.DoAttack(this, transform.position, charState.isRight, characterType);
    }
    IEnumerator WaitWhileAttack(string attackName)
    {
        yield return WaitWhileAnim(attackName);
        charState.inAttack = false;
    }

    public void GetHit(Hit hit)
    {
        bool wasDead = charState.IsDead;
        charState.health -= hit.damage;
        AddVelocityH(hit.GetForce);
        PlaySound(hitSounds);
        if (charState.IsDead)
        {
            if (!wasDead)
            {
                UpdateGUI(false);
                QuestController.OnEvent(new EventDescription
                {
                    who = hit.attackerType,
                    didWhat = EventType.kill,
                    toWhom = characterType
                });
                Anim_SetTrigger("die");
                OnDeath(hit);
                PlaySound(deathScreams);
            }
        }
        else
        {
            UpdateGUI(true);
            Anim_SetTrigger("hurt");
            OnHit(hit);
            PlaySound(hitScreams);
        }
    }
    protected virtual void OnHit(Hit hit) { }
    protected virtual void OnDeath(Hit hit)
    {
        gameObject.layer = LayerMask.NameToLayer(corpseLayer);
        faction = corpseFaction;
    }


    protected void PlaySound(ClipsCollection collection)
    {
        collection?.PlayRandomClip();
    }
    protected void UpdateGUI(bool isDisplayed)
    {
        characterGUI.UpdateUI(charState, isDisplayed);
    }


    public override void Flip_H()
    {
        base.Flip_H();
        charState.isRight = !charState.isRight;
    }

    protected Transform GetClosest(IEnumerable<Transform> transforms)
    {
        return transforms?.FirstOrDefault();
    }

    protected override void AddOnDrawGizmos()
    {
        base.AddOnDrawGizmos();
        if (attackStats != null) attackStats.OnGizmos(transform.position, charState.isRight);
    }
}