using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    private Dictionary<string, GameEventBase> _gameEvents = new Dictionary<string, GameEventBase>();
    private static GameEventManager _instance;
    public static GameEventManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    public void Register(GameEventBase gameEvent)
    {
        if (_gameEvents.ContainsKey(gameEvent.ID) == false)
        {
            _gameEvents.Add(gameEvent.ID, gameEvent);
        }
    }
    public void Unregister(GameEventBase gameEvent)
    {
        if (_gameEvents.ContainsKey(gameEvent.ID) == true)
        {
            _gameEvents.Remove(gameEvent.ID);
        }
    }
    public void TriggerEvent(string eventID)
    {
        bool isGameEventFound = _gameEvents.TryGetValue(eventID, out GameEventBase gameEvent);
        if (isGameEventFound == true)
        {
            gameEvent.Trigger();
        }
    }
    public void FinishEvent(string id)
    {
        bool isGameEventFound = _gameEvents.TryGetValue(id, out GameEventBase gameEvent);
        if (isGameEventFound == true)
        {
            gameEvent.Finish();
        }
    }
}
