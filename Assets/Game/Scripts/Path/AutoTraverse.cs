using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class AutoTraverse : MonoBehaviour
    {
        private Route route;
        private RouteTraverse traverse;
        [SerializeField] private float speed;

        private void Awake() {
            this.route = GetComponentInParent<Route>();
        }

        private void Start() {
            this.traverse = new RouteTraverse(route);
            this.transform.localPosition = traverse.LocalPosition;
        }

        private void FixedUpdate() {
            var distance = this.speed * Time.fixedDeltaTime;
            this.traverse.Advance(distance);
            this.transform.localPosition = this.traverse.LocalPosition;
        }
    }
}