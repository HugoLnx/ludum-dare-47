using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class RouteBounds : MonoBehaviour
    {
        private Route route;
        private BoxCollider2D box;

        private void Awake() {
            this.route = GetComponentInParent<Route>();
            this.box = GetComponent<BoxCollider2D>();
        }

        private void Start() {
            this.box.size = this.route.Bounds.size;
            this.box.offset = this.route.Bounds.center;
        }
    }
}