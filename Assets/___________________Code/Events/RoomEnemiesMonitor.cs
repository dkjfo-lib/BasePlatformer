using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomEnemiesMonitor : EventReceiver
{
    public FactionAlignment playerAlignment;
    [Space]
    public string[] StartListenEvents;
    public string[] ClearRoomEvents;
    protected override string[] ReceivedEvents => StartListenEvents.Concat(ClearRoomEvents).ToArray();
    [Space]
    public TagEmitting[] onRoomSuccessfullyCleanEvents;
    [Space]
    public List<NPC> allBots;
    public List<Pod> allPods;
    public bool isLookingForBots;

    public IEnumerable<NPC> EnemyNPCs => allBots.Where(s => s != null && playerAlignment.IsEnemy(s.state.alignment.faction));
    public IEnumerable<Pod> Pods => allPods.Where(s => s != null);


    protected override void OnEvent(string eventTag)
    {
        if (StartListenEvents.Contains(eventTag)) StartLookingForNPCs();
        if (ClearRoomEvents.Contains(eventTag)) ClearRoom();
    }

    void StartLookingForNPCs()
    {
        if (!isLookingForBots)
        {
            StartCoroutine(LookForNPCs());
        }
    }
    void ClearRoom()
    {
        isLookingForBots = false;
        foreach (var npc in EnemyNPCs)
        {
            npc.GetHit(new Hit
            {
                attackerType = ObjectType.ITEM_WEAPON,
                damage = 9999,
                force = 0,
                hitDirection = Vector2.right,
                hitPosition = Vector2.zero,
                isRight = false
            });
        }
        foreach (var pod in Pods)
        {
            pod.GetHit(new Hit
            {
                attackerType = ObjectType.ITEM_WEAPON,
                damage = 9999,
                force = 0,
                hitDirection = Vector2.right,
                hitPosition = Vector2.zero,
                isRight = false
            });
        }
    }

    IEnumerator LookForNPCs()
    {
        isLookingForBots = true;
        yield return new WaitUntil(() => EnemyNPCs.Count() > 0);

        yield return new WaitUntil(() => !isLookingForBots || EnemyNPCs.All(s => s.state.IsDead));
        if (isLookingForBots)
        {
            foreach (var tag in onRoomSuccessfullyCleanEvents)
            {
                tag.Emit(pipe_Events);
            }
            isLookingForBots = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var newBot = collision.gameObject.GetComponent<NPC>();
        if (newBot != null)
        {
            allBots.Add(newBot);
        }

        var newPod = collision.gameObject.GetComponent<Pod>();
        if (newPod != null)
        {
            allPods.Add(newPod);
        }
    }
}
