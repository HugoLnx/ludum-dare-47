using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class RoutePoint : MonoBehaviour
    {
        private const float LENGTH_PRECISION = 0.001f;
        public struct LerpResult {
            public Vector2 position;
            public float missing;
            public float traversed;
        }
        private Dictionary<RoutePoint, float> LengthCache = new Dictionary<RoutePoint, float>();
        public Vector2 LocalPosition => this.transform.localPosition;
        private Vector2 QuadracticRelPosition => this.transform.childCount < 1 ? Vector2.zero : Mathh.V3to2(this.transform.GetChild(0).localPosition);
        private Vector2 CubicRelPosition => this.transform.childCount < 2 ? Vector2.zero : Mathh.V3to2(this.transform.GetChild(1).localPosition);
        public Vector2 QuadracticLocalPosition => LocalPosition + QuadracticRelPosition;
        public Vector2 CubicLocalPosition => LocalPosition + CubicRelPosition;
        private bool IsLinear => QuadracticRelPosition == Vector2.zero && CubicRelPosition == Vector2.zero;
        private bool IsQuadractic => QuadracticRelPosition != Vector2.zero && CubicRelPosition == Vector2.zero;
        private bool IsCubic => CubicRelPosition != Vector2.zero;
        public LerpResult PathAbsLerpTo(RoutePoint next, float distance)
        {
            var length = PathLengthTo(next);
            var relDistance = distance / length;

            var result = new LerpResult();
            if (distance <= length) {
                result.position = PathRelLerpTo(next, relDistance);
                result.traversed = distance;
                result.missing = 0f;
            } else {
                result.traversed = length;
                result.missing = distance - result.traversed;
                result.position = next.LocalPosition;
            }
            return result;
        }

        public Vector2 PathRelLerpTo(RoutePoint next, float t) {
            if (IsLinear) {
                return ((1f-t)*this.LocalPosition) + (t*next.LocalPosition);
            } else if (IsQuadractic) {
                return ((1f-t)*(1f-t)*this.LocalPosition) + (2f*(1f-t)*t*QuadracticLocalPosition) + (t*t*next.LocalPosition);
            } else if (IsCubic) {
                return ((1f-t)*(1f-t)*(1f-t)*this.LocalPosition) + (3f*(1f-t)*(1f-t)*t*QuadracticLocalPosition) + (3f*(1f-t)*t*t*CubicLocalPosition) + (t*t*t*next.LocalPosition);
            }
            throw new Exception("I don't recognize this curve");
        }

        public Bounds PathBoundsTo(RoutePoint next)
        {
            var min = LocalPosition;
            var max = LocalPosition;
            min.x = Mathf.Min(min.x, next.LocalPosition.x);
            min.y = Mathf.Min(min.y, next.LocalPosition.y);
            max.x = Mathf.Min(max.x, next.LocalPosition.x);
            max.y = Mathf.Min(max.y, next.LocalPosition.y);
            if (!IsLinear) {
                var t = 0f;
                while (t < 1f) {
                    t += LENGTH_PRECISION;
                    var p = PathRelLerpTo(next, t);
                    min.x = Mathf.Min(min.x, p.x);
                    min.y = Mathf.Min(min.y, p.y);
                    max.x = Mathf.Max(max.x, p.x);
                    max.y = Mathf.Max(max.y, p.y);
                }
            }
            return new Bounds {min=min, max=max};
        }

        public float PathLengthTo(RoutePoint next)
        {
            if (LengthCache.ContainsKey(next)) return LengthCache[next];

            var length = 0f;

            if (IsLinear) {
                length = (this.LocalPosition - next.LocalPosition).magnitude;
            } else {
                var t = 0f;
                var lastPosition = this.LocalPosition;
                while (t < 1f) {
                    t += LENGTH_PRECISION;
                    var p = PathRelLerpTo(next, t);
                    length += (lastPosition - p).magnitude;
                    lastPosition = p;
                }
            }
            LengthCache[next] = length;
            return length;
        }

        public void DrawBezierGizmosTo(RoutePoint next)
        {
            GizmosUtils.OnColor(Color.green, () => {
                if (!IsLinear) {
                    Gizmos.DrawLine(this.LocalPosition, this.QuadracticLocalPosition);
                    Gizmos.DrawSphere(this.QuadracticLocalPosition, 0.08f);
                }
                if (IsCubic) {
                    Gizmos.DrawLine(next.LocalPosition, this.CubicLocalPosition);
                    Gizmos.DrawSphere(this.CubicLocalPosition, 0.08f);
                }
            });
        }
    }
}