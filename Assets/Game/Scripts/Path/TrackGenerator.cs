using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class TrackGenerator : MonoBehaviour
    {

        [SerializeField] private GameObject pointPrefab;
        [SerializeField] private float gap;

        private void Start() {
            var route = GetComponent<Route>();
            var trackContainer = new GameObject(name: "[GeneratedTrack]");
            trackContainer.transform.SetParent(this.transform);
            trackContainer.transform.localPosition = Vector2.zero;
            var traverse = new RouteTraverse(route);
            var pointsCount = Mathf.FloorToInt(route.Length / gap);
            var realGap = route.Length / pointsCount;
            for (var i = 0; i < pointsCount; i++) {
                var point = GameObject.Instantiate(pointPrefab, parent: trackContainer.transform);
                point.transform.localPosition = traverse.LocalPosition;
                traverse.Advance(realGap);
            }
        }
    }
}