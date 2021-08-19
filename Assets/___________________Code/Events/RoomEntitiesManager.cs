using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomEntitiesManager : EventReceiver
{
    public FactionAlignment playerAlignment;
    [Space]
    public string[] StartListeningOnEvents;
    public string[] ClearRoomOnEvents;
    protected override IEnumerable<string> ReceivedEvents => StartListeningOnEvents.Concat(ClearRoomOnEvents);
    [Space]
    public string[] onRoomSuccessfullyCleanEvents;
    [Space]
    public List<NPC> allBots;
    public List<Pod> allPods;
    public bool isLookingForBots;

    public IEnumerable<NPC> EnemyBots => allBots.Where(s => s != null && !s.state.IsDead && playerAlignment.IsEnemy(s.state.alignment.faction));
    public IEnumerable<Pod> Pods => allPods.Where(s => s != null);


    protected override void OnEvent(string eventTag)
    {
        if (StartListeningOnEvents.Contains(eventTag)) StartLookingForNPCs();
        if (ClearRoomOnEvents.Contains(eventTag)) StartCoroutine(ClearRoom());
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
            npc.GetHit(Hit.SelfDestroy);
        }
        foreach (var pod in Pods)
        {
            pod.GetHit(Hit.SelfDestroy);
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
            foreach (var eventTag in onRoomSuccessfullyCleanEvents)
            {
                pipe_Events.SendEvent(eventTag);
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
