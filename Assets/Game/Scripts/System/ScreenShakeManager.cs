using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    private Dictionary<string, ScreenShake> shakes = new Dictionary<string, ScreenShake>();
    private static ScreenShakeManager instance = null;
    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        }
        var allShakes = GetComponentsInChildren<ScreenShake>();
        foreach (var shake in allShakes) {
            shakes[shake.name] = shake;
        }
        instance = this;
    }

    public void ScreenShakeOn(string name, float duration = -1f) {
        shakes[name].Shake(duration);
    }

    public static void Shake(string name, float duration = -1f) {
        instance.ScreenShakeOn(name, duration);
    }
}
