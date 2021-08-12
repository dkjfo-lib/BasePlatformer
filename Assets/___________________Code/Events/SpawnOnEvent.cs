using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnOnEvent : EventReceiver
{
    public string[] SpawnOnEvents;
    public string[] LockOnEvents;
    public string[] UnLockOnEvents;
    protected override IEnumerable<string> ReceivedEvents => SpawnOnEvents.Concat(LockOnEvents).Concat(UnLockOnEvents);
    public bool isLocked = false;

    SpawnerOnce[] spawners;

    protected override void GetComponents()
    {
        spawners = GetComponentsInChildren<SpawnerOnce>();
    }

    protected override void OnEvent(string eventTag)
    {
        if (!isLocked && SpawnOnEvents.Contains(eventTag))
            foreach (var spawn in spawners)
            {
                spawn.PerformSpawn();
            }
        if (!isLocked && LockOnEvents.Contains(eventTag))
            isLocked = true;
        if (isLocked && UnLockOnEvents.Contains(eventTag))
            isLocked = false;
    }
}
