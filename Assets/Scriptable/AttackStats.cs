using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public AttackStats stats;

    [HideInInspector] public Vector2 offset = new Vector2(0, 0);
    private float timeLastAttack = -1;

    public void Init()
    {
        offset = stats.offset;
    }

    public bool DoAttack(Vector2 position)
    {
        bool canPerform = Time.timeSinceLevelLoad - timeLastAttack > stats.cooldown;
        if (canPerform)
        {
            timeLastAttack = Time.timeSinceLevelLoad;
            var hits = Physics2D.OverlapBoxAll(position + offset, stats.size, 0, stats.layerMask).Select(s => s.gameObject.GetComponent<CharacterAbstract>()).ToArray();
            foreach (var hit in hits)
            {
                hit.GetHit(new Hit
                {
                    damage = stats.damage,
                    position = position
                });
            }
        }
        return canPerform;
    }

    public void Flip_H()
    {
        offset = new Vector2(-offset.x, offset.y);
    }
}

[CreateAssetMenu(fileName = "NewAttackStats", menuName = "My/AttackStats")]
public class AttackStats : ScriptableObject
{
    public Vector2 offset = new Vector2(2, 0);
    public Vector2 size = new Vector2(2, .75f);
    public LayerMask layerMask;
    public int damage = 1;
    public float cooldown = 1;
}
