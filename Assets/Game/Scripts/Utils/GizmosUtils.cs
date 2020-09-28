using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosUtils
{
    public static void DrawArrow(Vector2 from, Vector2 to, float extendBy=1f)
    {
        var arrowVector = to - from;
        Gizmos.DrawLine(from, from + arrowVector*extendBy);
        var arrowHeadVector = (from - to).normalized;
        Gizmos.DrawLine(to, to + Mathh.RotateVector(arrowHeadVector, 30f, clockwise: true));
        Gizmos.DrawLine(to, to + Mathh.RotateVector(arrowHeadVector, 30f, clockwise: false));
    }

    public static void OnColor(Color c, Action act)
    {
        var oldColor = Gizmos.color;
        Gizmos.color = c;
        act();
        Gizmos.color = c;
    }
}
