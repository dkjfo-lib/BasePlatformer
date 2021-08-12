using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomBots : EventReceiver
{
    public Faction[] monitoringFactions;
    [Space]
    public List<NPC> allNPCs;

    public IEnumerable<NPC> Bots => allNPCs.Where(s => s != null);

    protected override IEnumerable<string> ReceivedEvents => monitoringFactions.Select(s => (int)s + "EnemyDetected");

    protected override void OnEvent(string eventTag)
    {
        foreach (var faction in monitoringFactions)
        {
            if (eventTag.StartsWith(((int)faction).ToString()))
            {
                var botsOfFaction = Bots.Where(s => s.state.alignment.faction == faction).Select(s => s).ToArray();
                var botsWithEnemy = botsOfFaction.Where(s => s.target.HasValue).Select(s => s).ToArray();

                var commandingBot = botsWithEnemy[Random.Range(0, botsWithEnemy.Length)];
                var botsWithoutEnemy = botsOfFaction.Where(s => !s.target.HasValue).Select(s => s).ToArray();
                foreach (var bot in botsWithoutEnemy)
                {
                    bot.lastEnemyPlace = commandingBot.target;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var newNPC = collision.gameObject.GetComponent<NPC>();
        if (newNPC == null) return;

        allNPCs.Add(newNPC);
    }
}
