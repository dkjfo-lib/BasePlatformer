using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Pipe_BetweenScenesData pipe_BetweenScenesData;
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            pipe_BetweenScenesData.UpdateData(this, player);
            ChangerScene();
        }
    }

    void ChangerScene()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
