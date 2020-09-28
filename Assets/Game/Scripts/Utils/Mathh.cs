using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction4Axis { UP, DOWN, LEFT, RIGHT }

public static class Mathh
{
    public static Vector2 Direction2Vector(Direction4Axis direction) {
        switch (direction) {
            case Direction4Axis.UP: return Vector2.up;
            case Direction4Axis.DOWN: return Vector2.down;
            case Direction4Axis.RIGHT: return Vector2.right;
            case Direction4Axis.LEFT: return Vector2.left;
        }
        return Vector2.zero;
    }
    public static float Trunc(float val, float min, float max) {
        val = Mathf.Min(max, val);
        val = Mathf.Max(min, val);
        return val;
    }

    public static int Trunc(int val, int min, int max) {
        val = Mathf.Min(max, val);
        val = Mathf.Max(min, val);
        return val;
    }

    public static Vector2 RotateVector(Vector2 vector, float degrees, bool clockwise=false)
    {
        var newAngle = Vector2Angle(vector) + degrees * (clockwise ? -1f : 1f);
        return Angle2Vector(newAngle).normalized * vector.magnitude;
    }

    public static Vector2 RotateVector(Vector2 vector, Quaternion quaternion)
        => RotateVector(vector, quaternion.eulerAngles.z);

    public static Vector2 Angle2Vector(float degrees) {
        var radians = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    public static Vector2 AbsVector(Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    public static Vector3 AbsVector(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

    public static float Vector2Angle(Vector2 vector) {
        return (Vector2.SignedAngle(Vector2.right, vector) + 360f*10f) % 360f;
    }

    public static float MagnitudeMax(params float[] values) {
        var result = values[0];
        foreach (var v in values) {
            if (Mathf.Abs(v) > Mathf.Abs(result)) {
                result = v;
            }
        }
        return result;
    }
    public static Vector2 MagnitudeMax(params Vector2[] vectors) {
        var result = vectors[0];
        foreach (var v in vectors) {
            if (v.magnitude > result.magnitude) {
                result = v;
            }
        }
        return result;
    }

    public static Vector2 V3to2(Vector3 v3) => (Vector2) v3;
    public static Vector3 V2to3(Vector2 v2) => new Vector3(v2.x, v2.y);
    public static float AngleFrom(Transform reference) {
        var angle = reference.rotation.eulerAngles.z;
        if (reference.lossyScale.x < 0f) angle = (angle + 180f) % 360f;
        return angle;
    }

    public static Vector2 DirectionFrom(Transform reference) => Angle2Vector(AngleFrom(reference));

    public static Vector2 NormalizeToStrongerDirection(Vector2 v)
    {
        var verticalIsStronger = (v * Vector2.up).magnitude > (v * Vector2.right).magnitude;
        if (verticalIsStronger) {
            if (v.y >= 0f) return Vector2.up;
            else return Vector2.down;
        } else {
            if (v.x >= 0f) return Vector2.right;
            else return Vector2.left;
        }
    }

    public static Vector2 FlipX(Vector2 v) => new Vector2(-v.x, v.y);
}
