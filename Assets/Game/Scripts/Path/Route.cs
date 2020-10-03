using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class Route : MonoBehaviour
    {
        public RoutePoint[] Points {get; private set;}
        private float Length;


        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            var count = this.transform.childCount;
            this.Points = new RoutePoint[count];
            for (var i = 0; i < count; i++)
            {
                var pointTransform = this.transform.GetChild(i);
                Points[i] = Utils.MaybeAddComponent<RoutePoint>(pointTransform.gameObject);
            }
            this.Length = CalculateLength();
        }

        private void OnDrawGizmos() {
            Setup();

            var minGapSize = 0.5f;
            var dotsCount = Mathf.FloorToInt(Length / minGapSize);
            var gapSize = Length / dotsCount;
            var traverse = new RouteTraverse(this);

            for (var i = 0; i < dotsCount; i++) {
                Gizmos.DrawSphere(traverse.Position, 0.05f);
                traverse.Advance(gapSize);
            }

            for (var i = 0; i < Points.Length; i++) {
                var current = Points[i];
                var next = Points[(i+1) % Points.Length];
                current.DrawBezierGizmosTo(next);
            }
        }

        private float CalculateLength()
        {
            var length = 0f;
            for (var i = 0; i < Points.Length; i++) {
                var current = Points[i];
                var next = Points[(i+1) % Points.Length];
                length += current.PathLengthTo(next);
            }
            return length;
        }
    }
}