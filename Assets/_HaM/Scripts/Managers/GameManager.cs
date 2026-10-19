using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Prepare, PrepareGame);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, PrepareGame);
    }

    private void PrepareGame()
    {
        EventManager.TriggerEvent(Consts.Events.Game.Started);
    }
}
