using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spawn
{
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