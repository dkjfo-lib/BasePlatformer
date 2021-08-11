using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomBots : MonoBehaviour
{
    public Faction[] monitoringFactions;
    [Space]
    public List<NPC> allNPCs;

    public IEnumerable<NPC> Bots => allNPCs.Where(s => s != null);

    private void Start()
    {
        foreach (var faction in monitoringFactions)
        {
            StartCoroutine(MonitorBotsOfFaction(faction));
        }
    }

    IEnumerator MonitorBotsOfFaction(Faction faction)
    {
        NPC[] botsOfFaction;
        NPC[] botsWithEnemy;

        while (true)
        {
            do
            {
                yield return new WaitForSeconds(.5f);
                botsOfFaction = Bots.Where(s => s.state.alignment.faction == faction).Select(s => s).ToArray();
                botsWithEnemy = botsOfFaction.Where(s => s.target.HasValue).Select(s => s).ToArray();
            } while (botsWithEnemy.Length == 0);

            var commandingBot = botsWithEnemy[Random.Range(0, botsWithEnemy.Length)];
            var botsWithoutEnemy = botsOfFaction.Where(s => !s.target.HasValue).Select(s => s).ToArray();
            foreach (var bot in botsWithoutEnemy)
            {
                bot.lastEnemyPlace = commandingBot.target;
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var newNPC = collision.gameObject.GetComponent<NPC>();
        if (newNPC == null) return;

        allNPCs.Add(newNPC);
    }
}
