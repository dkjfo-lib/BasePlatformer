using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BetweenScenesData", menuName = "Pipes/BetweenScenesData")]
public class Pipe_BetweenScenesData : ScriptableObject
{
    public float positionY;
    public Vector2 velocity;
    public int hp = 10;
    public bool isRight;

    public void UpdateData(SceneChanger sceneChanger, Player player)
    {
        positionY = player.transform.position.y - sceneChanger.transform.position.y;
        hp = player.charState.health;
        isRight = player.charState.isRight;
        velocity = player.Velocity;
    }

    public void ApplyData(Player player)
    {
        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + positionY);
        player.charState.health = hp;
        if (isRight != player.charState.isRight)
            player.Flip_H();
        player.SetVelocity(velocity);
    }
}
