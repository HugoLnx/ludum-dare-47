using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour
{
    private static ScreenFreeze instance;
    private void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }
    public void InstanceFreeze(float duration, System.Action after = null, float delay = 0f, float scale = 0f) {
        StartCoroutine(FreezeCoroutine(duration, after, delay, scale));
    }

    public static void Freeze(float duration, System.Action after = null, float delay = 0f, float scale = 0f)
        => instance.InstanceFreeze(duration, after, delay, scale);

    private IEnumerator FreezeCoroutine(float duration, Action after, float delay, float scale)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        if (after != null) after();
    }
}
