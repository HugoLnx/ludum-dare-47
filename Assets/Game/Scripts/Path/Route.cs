using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class Route : MonoBehaviour
    {
        private const float MIN_GAP_SIZE = 0.5f;

        [SerializeField] private Transform pointsContainer;

        public RoutePoint[] Points {get; private set;}
        public float Length {get; private set;}
        public Bounds Bounds { get; private set; }
        public bool IsValid => this.pointsContainer?.childCount >= 2;


        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            if (!IsValid) return;
            var count = this.pointsContainer.childCount;
            this.Points = new RoutePoint[count];
            for (var i = 0; i < count; i++)
            {
                var pointTransform = this.pointsContainer.GetChild(i);
                Points[i] = Utils.MaybeAddComponent<RoutePoint>(pointTransform.gameObject);
            }
            PrecalculateInfo();
        }

        private void OnDrawGizmos() {
            if (!IsValid) return;
            Setup();

            var dotsCount = Mathf.FloorToInt(Length / MIN_GAP_SIZE);
            var gapSize = Length / dotsCount;
            var traverse = new RouteTraverse(this);

            for (var i = 0; i < dotsCount; i++) {
                Gizmos.DrawSphere(traverse.LocalPosition, 0.05f);
                traverse.Advance(gapSize);
            }

            for (var i = 0; i < Points.Length; i++) {
                var current = Points[i];
                var next = Points[(i+1) % Points.Length];
                current.DrawBezierGizmosTo(next);
            }
        }

        private void PrecalculateInfo()
        {
            var length = 0f;
            var min = Points[0].LocalPosition;
            var max = Points[0].LocalPosition;
            for (var i = 0; i < Points.Length; i++) {
                var current = Points[i];
                var next = Points[(i+1) % Points.Length];
                length += current.PathLengthTo(next);
                var bounds = current.PathBoundsTo(next);
                min.x = Mathf.Min(min.x, bounds.min.x);
                min.y = Mathf.Min(min.y, bounds.min.y);
                max.x = Mathf.Max(max.x, bounds.max.x);
                max.y = Mathf.Max(max.y, bounds.max.y);
            }
            this.Length =  length;
            this.Bounds = new Bounds{min=min, max=max};
        }
    }
}