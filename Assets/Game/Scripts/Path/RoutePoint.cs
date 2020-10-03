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
        public Vector2 Position => this.transform.localPosition;
        private Vector2 QuadracticRelPosition => this.transform.childCount < 1 ? Vector2.zero : Mathh.V3to2(this.transform.GetChild(0).localPosition);
        private Vector2 CubicRelPosition => this.transform.childCount < 2 ? Vector2.zero : Mathh.V3to2(this.transform.GetChild(1).localPosition);
        public Vector2 QuadracticPosition => Position + QuadracticRelPosition;
        public Vector2 CubicPosition => Position + CubicRelPosition;
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
                result.position = next.Position;
            }
            return result;
        }

        public Vector2 PathRelLerpTo(RoutePoint next, float t) {
            if (IsLinear) {
                return ((1f-t)*this.Position) + (t*next.Position);
            } else if (IsQuadractic) {
                return ((1f-t)*(1f-t)*this.Position) + (2f*(1f-t)*t*QuadracticPosition) + (t*t*next.Position);
            } else if (IsCubic) {
                return ((1f-t)*(1f-t)*(1f-t)*this.Position) + (3f*(1f-t)*(1f-t)*t*QuadracticPosition) + (3f*(1f-t)*t*t*CubicPosition) + (t*t*t*next.Position);
            }
            throw new Exception("I don't recognize this curve");
        }

        public float PathLengthTo(RoutePoint next)
        {
            if (IsLinear) return (this.Position - next.Position).magnitude;

            var length = 0f;
            var t = 0f;
            var lastPosition = this.Position;
            while (t < 1f) {
                t += LENGTH_PRECISION;
                var p = PathRelLerpTo(next, t);
                length += (lastPosition - p).magnitude;
                lastPosition = p;
            }
            return length;
        }

        public void DrawBezierGizmosTo(RoutePoint next)
        {
            GizmosUtils.OnColor(Color.green, () => {
                if (!IsLinear) {
                    Gizmos.DrawLine(this.Position, this.QuadracticPosition);
                    Gizmos.DrawSphere(this.QuadracticPosition, 0.08f);
                }
                if (IsCubic) {
                    Gizmos.DrawLine(next.Position, this.CubicPosition);
                    Gizmos.DrawSphere(this.CubicPosition, 0.08f);
                }
            });
        }
    }
}