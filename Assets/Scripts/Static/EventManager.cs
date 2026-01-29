using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static Dictionary<Type, Delegate> eventTable = new Dictionary<Type, Delegate>();

    // Event dinleme
    public static void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (eventTable.TryGetValue(eventType, out Delegate existingDelegate))
        {
            eventTable[eventType] = Delegate.Combine(existingDelegate, listener);
        }
        else
        {
            eventTable[eventType] = listener;
        }
    }

    // Event'ten çýkma
    public static void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (eventTable.TryGetValue(eventType, out Delegate existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, listener);

            if (newDelegate == null)
            {
                eventTable.Remove(eventType);
            }
            else
            {
                eventTable[eventType] = newDelegate;
            }
        }
    }

    // Event tetikleme
    public static void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (eventTable.TryGetValue(eventType, out Delegate existingDelegate))
        {
            if (existingDelegate is Action<T> callback)
            {
                callback.Invoke(eventData);
            }
        }
    }
}
