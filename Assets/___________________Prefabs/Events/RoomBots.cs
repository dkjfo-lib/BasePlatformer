using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RoomEntitiesManager))]
public class RoomBots : EventReceiver
{
    public Faction[] monitoringFactions;

    public IEnumerable<NPC> Bots => entitiesManager.allBots.Where(s => s != null && !s.state.IsDead);
    protected override IEnumerable<string> ReceivedEvents => monitoringFactions.Select(s => (int)s + "EnemyDetected");
    
    RoomEntitiesManager entitiesManager;

    protected override void GetComponents()
    {
        base.GetComponents();
        entitiesManager = GetComponent<RoomEntitiesManager>();
    }

    protected override void OnEvent(string eventTag)
    {
        foreach (var faction in monitoringFactions)
        {
            if (eventTag.StartsWith(((int)faction).ToString()))
            {
                var botsOfFaction = Bots.Where(s => s.state.alignment.faction == faction).Select(s => s).ToArray();
                var botsWithEnemy = botsOfFaction.Where(s => s.seesTarget).Select(s => s).ToArray();
                if (botsWithEnemy.Length == 0) return;

                var commandingBot = botsWithEnemy[Random.Range(0, botsWithEnemy.Length)];
                var botsWithoutEnemy = botsOfFaction.Where(s => !s.seesTarget).Select(s => s).ToArray();
                foreach (var bot in botsWithoutEnemy)
                {
                    bot.lastEnemyPosition = commandingBot.lastEnemyPosition;
                    bot.isLookingForTarget = true;
                }
            }
        }
    }
}
