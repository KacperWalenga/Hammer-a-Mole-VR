using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    private Dictionary<string, Action> events;
    private Dictionary<string, Action<object>> paramEvents;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        events = new Dictionary<string, Action>();
        paramEvents = new Dictionary<string, Action<object>>();
    }

    public static void StartListening(string eventName, Action listener)
    {
        if (!instance)
        {
            Debug.LogWarning("EventManager not found");
            return;
        }

        if (instance.events.TryGetValue(eventName, out var action))
        {
            action += listener;
            instance.events[eventName] = action;
        }
        else
        {
            action += listener;
            instance.events.Add(eventName, action);
        }
    }

    public static void StartListening(string eventName, Action<object> listener)
    {
        if (!instance)
        {
            Debug.LogWarning("EventManager not found");
            return;
        }

        if (instance.paramEvents.TryGetValue(eventName, out var action))
        {
            action += listener;
            instance.paramEvents[eventName] = action;
        }
        else
        {
            action += listener;
            instance.paramEvents.Add(eventName, action);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (!instance){
            Debug.LogWarning("EventManager not found");
            return;
        }
        
        if(!instance.events.TryGetValue(eventName, out var action))
            return;
        
        action -= listener;
        instance.events[eventName] = action;
    }

    public static void StopListening(string eventName, Action<object> listener)
    {
        if (!instance){
            Debug.LogWarning("EventManager not found");
            return;
        }
        
        if(!instance.paramEvents.TryGetValue(eventName, out var action))
            return;
        
        action -= listener;
        instance.paramEvents[eventName] = action;
    }

    public static void TriggerEvent(string eventName)
    {
        if (!instance)
        {
            Debug.LogWarning("EventManager not found");
            return;
        }
        
        if(instance.events.TryGetValue(eventName, out var action))
            action?.Invoke();
    }

    public static void TriggerEvent(string eventName, object param)
    {
        if (!instance)
        {
            Debug.LogWarning("EventManager not found");
            return;
        }
        
        if(instance.paramEvents.TryGetValue(eventName, out var action))
            action?.Invoke(param);
    }
}