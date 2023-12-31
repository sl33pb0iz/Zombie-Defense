using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Spicyy.System
{
    public abstract class GameEvent
    {
        public abstract void ResetAll();
    }

    public static class EventManager
    {
        private static readonly Dictionary<Type, List<UnityAction<GameEvent>>> s_Events = new Dictionary<Type, List<UnityAction<GameEvent>>>();
        private static readonly Dictionary<Delegate, UnityAction<GameEvent>> s_EventLookups = new Dictionary<Delegate, UnityAction<GameEvent>>();

        public static void AddListener<T>(UnityAction<T> evt, int index = -1) where T : GameEvent
        {
            if (!s_EventLookups.ContainsKey(evt))
            {
                void newAction(GameEvent e) => evt((T)e);
                s_EventLookups[evt] = newAction;

                if (s_Events.TryGetValue(typeof(T), out var internalList))
                {
                    if (index >= 0 && index < internalList.Count)
                    {
                        if (internalList[index] == null)
                        {
                            // Create a new list and insert the action at the specified index
                            internalList[index] = newAction;
                        }
                        else
                        {
                            // If there's an existing list, add the new action to the list
                            internalList[index] += newAction;
                        }
                    }
                    else if (index > internalList.Count)
                    {
                        while (internalList.Count < index)
                        {
                            internalList.Add(null);
                        }
                        internalList.Add(newAction);
                    }
                    else
                    {
                        internalList.Add(newAction);
                    }
                }
                else
                {
                    s_Events[typeof(T)] = new List<UnityAction<GameEvent>>(new[] { (UnityAction<GameEvent>)newAction });
                }
            }
        }

        public static void RemoveListener<T>(UnityAction<T> evt) where T : GameEvent
        {
            if (s_EventLookups.TryGetValue(evt, out var action))
            {
                if (s_Events.TryGetValue(typeof(T), out var tempActionList))
                {
                    tempActionList.Remove(action);
                    if (tempActionList.Count == 0)
                        s_Events.Remove(typeof(T));
                }

                s_EventLookups.Remove(evt);
            }
        }

        // chạy hàm này để invoke các action trong game
        public static void Broadcast(GameEvent evt)
        {
            if (s_Events.TryGetValue(evt.GetType(), out var actionList))
            {
                foreach (var action in actionList)
                {
                    action?.Invoke(evt);
                }
            }
        }

        // Clear hết tất cả trước khi load level mới
        public static void Clear()
        {
            s_Events.Clear();
            s_EventLookups.Clear();
        }
    }
}
