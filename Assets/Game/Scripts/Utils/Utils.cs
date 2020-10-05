using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class Utils
{
    public static Color ColorWithAlpha(Color c, float alpha) {
        return new Color(c.r, c.g, c.b, alpha);
    }

    public static Dictionary<string, Transform> BuildChildrenDictionary(Transform root)
    {
        var dict = new Dictionary<string, Transform>();
        for (var i = 0; i < root.childCount; i++) {
            var child = root.GetChild(i);
            dict[child.name] = child;
        }
        return dict;
    }

    public static void EachChild(Transform transform, Action<Transform> func)
    {
        var count = transform.childCount;
        for (var i = 0; i < count; i++) {
            func(transform.GetChild(i));
        }
    }
    public static void EachChild(GameObject obj, Action<Transform> func)
        => EachChild(obj.transform, func);
    public static void EachChild(MonoBehaviour mono, Action<Transform> func)
        => EachChild(mono.transform, func);

    public static T RandomEnum<T>()
    {
        var values = Enum.GetValues(typeof(T));
        return (T) values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    public static void FlipX(Transform transform)
    {
        var s = transform.localScale;
        transform.localScale = new Vector3(s.x*-1f, s.y, s.z);
    }

    public static void FlipY(Transform transform)
    {
        var s = transform.localScale;
        transform.localScale = new Vector3(s.x, s.y*-1f, s.z);
    }

    public static T ChooseRandomly<T>(params T[] elements) {
        var i = Mathf.FloorToInt(elements.Length * (UnityEngine.Random.Range(0f, 0.999f)));
        return elements[i];
    }

    public static Vector2[] PointsForSemiCircle(float radius, int amount, float angleRange, Vector2 origin, float initialAngle = 0f, bool clockwise = true, float maxVariation=0f) {
        var direction = Mathh.Angle2Vector(initialAngle);
        var pointer = direction.normalized * radius;
        var points = new Vector2[amount];
        if (amount >= 1) points[0] = origin + VariatePointer(pointer, maxVariation, clockwise);
        if (amount >= 2) points[amount-1] = origin + VariatePointer(Mathh.RotateVector(pointer, angleRange, clockwise), maxVariation, clockwise);
        if (amount >= 3) {
            var angleBetween = angleRange / (amount-1);
            for (var i = 1; i < amount-1; i++) {
                pointer = Mathh.RotateVector(pointer, angleBetween, clockwise);
                points[i] = origin + VariatePointer(pointer, maxVariation, clockwise);
            }
        }
        return points;
    }

    public static void MaybeSetBool(Animator animator, string paramName, bool value)
    {
        var hasParameter = false;
        foreach (var param in animator.parameters) {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Bool) {
                hasParameter = true;
                break;
            }
        }
        if (!hasParameter) return;
        animator.SetBool(paramName, value);
    }

    public static T MaybeAddComponent<T>(GameObject obj)
    where T:Component
    {
        var comp = obj.GetComponent<T>();
        if (comp == null) comp = obj.AddComponent<T>();
        return comp;
    }

    private static Vector2 VariatePointer(Vector2 pointer, float maxVariation, bool clockwise)
    {
        if (maxVariation == 0f) return pointer;
        var random = UnityEngine.Random.value;
        var angle = maxVariation * random - maxVariation/2f;
        return Mathh.RotateVector(pointer, angle, clockwise);
    }

    public static float RandomMoreOrLess(float v) => UnityEngine.Random.Range(1f-v, 1f+v);

    public static Color ColorIntensify(Color color, float v)
    {
        var sumVal = color.r + color.g + color.b;
        var weights = new Dictionary<string, float> {
          {"red", color.r/sumVal},
          {"green", color.g/sumVal},
          {"blue", color.b/sumVal}
        };
        var ordered = weights.Keys.OrderByDescending(k => weights[k]).ToArray();
        var scale = 1f/weights[ordered[0]];
        var rInx = ordered.Select((n,i) => (n, i)).Where(t => t.Item1 == "red").First().Item2;
        var gInx = ordered.Select((n,i) => (n, i)).Where(t => t.Item1 == "green").First().Item2;
        var bInx = ordered.Select((n,i) => (n, i)).Where(t => t.Item1 == "blue").First().Item2;
        var intense = new Color(
            color.r * (scale - scale * (0.3f * rInx)),
            color.g * (scale - scale * (0.3f * gInx)),
            color.b * (scale - scale * (0.3f * bInx)),
            color.a
        );
        return Color.Lerp(color, intense, v);
    }

    public static Color ColorDarken(Color color, float v)
    {
        return Color.Lerp(color, Color.black, v);
    }

    public static void OnHasValue<T>(T? nullableValue, Action<T> func) where T:struct
    {
        if (nullableValue.HasValue) func(nullableValue.Value);
    }

    public static void OnBothHaveValues<T>(T? nullable1, T? nullable2, Action<T, T> func) where T:struct
    {
        if (!nullable1.HasValue || !nullable2.HasValue) return;
        func(nullable1.Value, nullable2.Value);
    }

    public static void Set2DRotation(Transform transform, float angle)
    {
        var euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(euler.x, euler.y, angle);
    }
    public static void Rotate2D(Transform transform, float angle) {
        Set2DRotation(transform, transform.rotation.eulerAngles.z+angle);
    }

    public static void SetPosition(Transform transform, float? x = null, float? y = null, float? z = null) {
        transform.position = new Vector3(
            x.HasValue ? x.Value : transform.position.x,
            y.HasValue ? y.Value : transform.position.y,
            z.HasValue ? z.Value : transform.position.z
        );
    }

    public static void SetLocalPosition(Transform transform, float? x = null, float? y = null, float? z = null) {
        transform.localPosition = new Vector3(
            x.HasValue ? x.Value : transform.localPosition.x,
            y.HasValue ? y.Value : transform.localPosition.y,
            z.HasValue ? z.Value : transform.localPosition.z
        );
    }

    public static void SetPosition(MonoBehaviour mono, float? x = null, float? y = null, float? z = null)
        => SetPosition(mono.transform, x, y, z);

    public static void SetPosition(GameObject obj, float? x = null, float? y = null, float? z = null)
        => SetPosition(obj.transform, x, y, z);

    public static Tween TweenRotate2DTo(Transform transform, float angle, float duration, bool clockwise, float? fromAngle = null)
    {
        angle = (angle + 360f) % 360f;
        var currentAngle = fromAngle.HasValue ? fromAngle.Value : transform.rotation.eulerAngles.z;
        var rotation = (angle - currentAngle + 360f) % 360f - (clockwise ? 360f : 0f);
        return transform.DORotate(new Vector3(0f, 0f, rotation), duration, mode: RotateMode.WorldAxisAdd);
    }

    public static string ReusableID(Transform objTransform)
    {
        var t = objTransform;
        var path = new List<string>();
        path.Add(t.GetSiblingIndex().ToString());
        while (t != null) {
            path.Add(t.name);
            t = t.parent;
        }
        path.Add(SceneManager.GetActiveScene().name);
        path.Reverse();
        return String.Join("/", path);
    }

    private const float WAIT_DELAY = 0.1f;
    public static IEnumerator WaitWhile(Func<bool> condition, float timeout = -99999f) {
        while (condition() && timeout > 0f) {
            yield return new WaitForSecondsRealtime(WAIT_DELAY);
            timeout -= WAIT_DELAY;
        }
        if (timeout <= 0 && timeout > -99999f) {
            Debug.Log("WaitWhile Timedout");
        }
    }

    public static IEnumerator WaitUntil(Func<bool> condition, float timeout = -99999f) {
        yield return WaitWhile(() => !condition(), timeout: timeout);
    }

    public static TimeSpan QBench(string name, Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        Debug.Log($"Benchmark {name}: {stopwatch.Elapsed}");
        return stopwatch.Elapsed;
    }


    public class Bench {
        private string name;
        private Stopwatch stopwatch;

        public Bench(string name) {
            this.name = name;
            this.stopwatch = Stopwatch.StartNew();
        }

        public TimeSpan Stop() {
            stopwatch.Stop();
            UnityEngine.Debug.Log($"Benchmark {name}: {stopwatch.ElapsedMilliseconds}");
            return stopwatch.Elapsed;
        }
    }
}