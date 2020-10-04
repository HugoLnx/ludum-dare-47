using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class RouteTraverse
    {
        private readonly Route route;
        private int inx;
        private float traversed;

        private RoutePoint[] Points => route.Points;
        private RoutePoint Current;
        private RoutePoint Next;
        public Vector2 LocalPosition => Current.PathAbsLerpTo(Next, traversed).position;

        public RouteTraverse(Route route) {
            this.route = route;
            this.inx = 0;
            this.traversed = 0f;
            this.Current = Points[inx];
            this.Next = Points[(inx+1) % Points.Length];
        }

        public void Advance(float distance) {
            while (true) {
                var lerp = Current.PathAbsLerpTo(Next, traversed+distance);
                distance = lerp.missing;
                traversed = lerp.traversed;
                if (lerp.missing == 0f) {
                    break;
                } else {
                    inx = (inx+1) % Points.Length;
                    Current = Points[inx];
                    Next = Points[(inx+1) % Points.Length];
                    traversed = 0f;
                }
            }
        }
    }
}