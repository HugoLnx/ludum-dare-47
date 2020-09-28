using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

public class GameEventListener<T> : MonoBehaviour
where T : GameEvent<T>
{
    public delegate void EventAction(T evt);
    public EventAction action;
    public UnityEvent unityEvent;
    private T evt;

    private void Awake() {
        evt = typeof(T).GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy).GetValue(null, null) as T;
    }

    private void OnEnable() {
        if (evt == null) return;
        evt.hooks += OnCall;
    }

    private void OnDisable() {
        if (evt == null) return;
        evt.hooks -= OnCall;
    }

    private void OnCall() {
        if (action != null) action(evt);
        if (unityEvent != null) unityEvent.Invoke();
    }
}
