using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AVisibleCounter : MonoBehaviour
{
    public static int visibleCount = 0;

    private void OnBecameVisible() {
        visibleCount++;
    }
    private void OnBecameInvisible() {
        visibleCount--;
    }
}
