using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomEnemiesMonitor : EventActivation
{
    public FactionAlignment playerAlignment;
    [Space]
    public string[] StartListenEvents;
    public string[] ClearRoomEvents;
    protected override string[] ActivationEvents => StartListenEvents.Concat(ClearRoomEvents).ToArray();
    [Space]
    public TagEmitting[] onRoomSuccessfullyCleanEvents;
    [Space]
    public List<NPC> allNPCs;
    public bool isLookingForNPCs;

    public IEnumerable<NPC> EnemyNPCs => allNPCs.Where(s => s != null && playerAlignment.IsEnemy(s.state.alignment.faction));


    protected override void Activate(string eventTag)
    {
        if (StartListenEvents.Contains(eventTag)) StartLookingForNPCs();
        if (ClearRoomEvents.Contains(eventTag)) ClearRoom();
    }

    void StartLookingForNPCs()
    {
        if (!isLookingForNPCs)
        {
            StartCoroutine(LookForNPCs());
        }
    }
    void ClearRoom()
    {
        isLookingForNPCs = false;
        foreach (var npc in EnemyNPCs)
        {
            Destroy(npc.gameObject, .5f);
        }
    }

    IEnumerator LookForNPCs()
    {
        isLookingForNPCs = true;
        yield return new WaitUntil(() => EnemyNPCs.Count() > 0);

        yield return new WaitUntil(() => !isLookingForNPCs || EnemyNPCs.All(s => s.state.IsDead));
        if (isLookingForNPCs)
        {
            foreach (var tag in onRoomSuccessfullyCleanEvents)
            {
                tag.Emit(pipe_Events);
            }
            isLookingForNPCs = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var newNPC = collision.gameObject.GetComponent<NPC>();
        if (newNPC == null) return;

        allNPCs.Add(newNPC);
    }
}
