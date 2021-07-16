using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public AttackStats stats;
    public bool isRight = true;

    [HideInInspector] public Vector2 offset = new Vector2(0, 0);
    private float timeLastAttack = -1;

    public void Init()
    {
        offset = stats.offset;
        if (!isRight) Flip_H();
    }

    public bool CanAttack()
    {
        bool cooldownIsOver = Time.timeSinceLevelLoad - timeLastAttack > stats.cooldown;
        return cooldownIsOver;
    }

    public CharacterAbstract[] CastAttack(Vector2 position)
    {
        var hits = Physics2D.OverlapBoxAll(position + offset, stats.size, 0, stats.layerMask);
        var characterHits = hits.Select(s => s.gameObject.GetComponent<CharacterAbstract>()).ToArray();
        return characterHits;
    }

    public void DoAttack(Vector2 position)
    {
        if (CanAttack())
        {
            timeLastAttack = Time.timeSinceLevelLoad;
            var hits = CastAttack(position);
            foreach (var hit in hits)
            {
                hit.GetHit(new Hit
                {
                    damage = stats.damage,
                    force = stats.force,
                    position = position
                });
            }
        }
    }

    public void Flip_H()
    {
        offset = new Vector2(-offset.x, offset.y);
    }
    public void OnGizmos(Vector2 position)
    {
        if (stats == null) return;
        if (offset == Vector2.zero)
        {
            if (isRight)
                Gizmos.DrawWireCube(position + stats.offset, stats.size);
            else
                Gizmos.DrawWireCube(position - stats.offset, stats.size);
        }
        else
        {
            if (isRight)
                Gizmos.DrawWireCube(position + offset, stats.size);
            else
                Gizmos.DrawWireCube(position - offset, stats.size);
        }
    }
}

[CreateAssetMenu(fileName = "NewAttackStats", menuName = "My/AttackStats")]
public class AttackStats : ScriptableObject
{
    public Vector2 offset = new Vector2(2, 0);
    public Vector2 size = new Vector2(2, .75f);
    public LayerMask layerMask;
    public int damage = 1;
    public int force = 1000;
    public float cooldown = 1;
}
