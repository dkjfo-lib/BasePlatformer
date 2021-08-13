using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomEntitiesManager : EventReceiver
{
    public FactionAlignment playerAlignment;
    [Space]
    public string[] StartListenEvents;
    public string[] ClearRoomEvents;
    protected override IEnumerable<string> ReceivedEvents => StartListenEvents.Concat(ClearRoomEvents);
    [Space]
    public TagEmitting[] onRoomSuccessfullyCleanEvents;
    [Space]
    public List<NPC> allBots;
    public List<Pod> allPods;
    public bool isLookingForBots;

    public IEnumerable<NPC> EnemyBots => allBots.Where(s => s != null && !s.state.IsDead && playerAlignment.IsEnemy(s.state.alignment.faction));
    public IEnumerable<Pod> Pods => allPods.Where(s => s != null);


    protected override void OnEvent(string eventTag)
    {
        if (StartListenEvents.Contains(eventTag)) StartLookingForNPCs();
        if (ClearRoomEvents.Contains(eventTag)) StartCoroutine(ClearRoom());
    }

    void StartLookingForNPCs()
    {
        if (!isLookingForBots)
        {
            isLookingForBots = true;
            StartCoroutine(LookForNPCs());
        }
    }
    IEnumerator ClearRoom()
    {
        isLookingForBots = false;
        foreach (var npc in EnemyBots)
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
        yield return new WaitForSeconds(.1f);
        allPods = allPods.Where(s => s != null).ToList();
        allBots = allBots.Where(s => s != null).ToList();
    }

    IEnumerator LookForNPCs()
    {
        yield return new WaitUntil(() => EnemyBots.Count() > 0);

        yield return new WaitUntil(() => !isLookingForBots || EnemyBots.All(s => s.state.IsDead));
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
