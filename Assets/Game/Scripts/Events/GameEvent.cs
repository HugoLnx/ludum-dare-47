using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public abstract class GameEvent<T>
where T : GameEvent<T>
{
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = System.Activator.CreateInstance(typeof(T)) as T;
            }
            return instance;
        }
    }
    public delegate void EventHook();
    public event EventHook hooks;

    public void Emit() {
        if (hooks != null) hooks.Invoke();
    }
}