using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BaseExt
{
    public static bool ShouldHit(Limb limb, IHittable hitted)
    {
        if (hitted == null) return false;

        bool isOneself = limb.Father == (Object)hitted;
        bool selfDamageIsOn = limb.weapon.stats.isSelfDamageOn.value;
        bool shoulHitSelf = isOneself && selfDamageIsOn;
        if (shoulHitSelf) return true;

        bool friendlyFireIsOn = limb.weapon.stats.isFriendlyDamageOn.value;
        bool isEnemy = limb.Father.state.alignment.IsEnemy(hitted.Faction);
        bool shoulHitOther = !isOneself && (friendlyFireIsOn || isEnemy);
        if (shoulHitOther) return true;

        bool alwaysHitted = hitted.Faction == Faction.Item_AllDamage;
        if (alwaysHitted) return alwaysHitted;
        return false;
    }

    public static bool ShouldHit(Projectile projectile, IHittable hitted)
    {
        if (hitted == null) return false;

        bool isOneself = projectile == (Object)hitted;
        bool selfDamageIsOn = projectile.stats.isSelfDamageOn.value;
        bool shoulHitSelf = isOneself && selfDamageIsOn;
        if (shoulHitSelf) return true;

        bool friendlyFireIsOn = projectile.stats.isFriendlyDamageOn.value;
        bool isEnemy = projectile.state.alignment.IsEnemy(hitted.Faction);
        bool shoulHitOther = !isOneself && (friendlyFireIsOn || isEnemy);
        if (shoulHitOther) return true;

        bool alwaysHitted = hitted.Faction == Faction.Item_AllDamage;
        if (alwaysHitted) return alwaysHitted;
        return false;
    }

    public static void SpawnParticles(Transform host, ParticleSystem particles, bool isRight)
    {
        if (particles != null)
        {
            var newParticle = GameObject.Instantiate(particles,
                host.position + Vector3.up * .5f,
                isRight
                    ? particles.transform.rotation
                    : Quaternion.Euler(180 - particles.transform.rotation.eulerAngles.x, particles.transform.rotation.eulerAngles.y, particles.transform.rotation.eulerAngles.z),
                host.parent);
            GameObject.Destroy(newParticle.gameObject, 2);
        }
    }
}