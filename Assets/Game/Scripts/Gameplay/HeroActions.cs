using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD47 {
    public class HeroActions : MonoBehaviour
    {
        private const string TAG_TRAP = "Trap";
        [SerializeField] private GameObject explodePrefab;
        private void OnTriggerEnter2D(Collider2D other) => OnCollision(other);
        private void OnTriggerStay2D(Collider2D other) => OnCollision(other);

        private void OnCollision(Collider2D other)
        {
            if (other.CompareTag(TAG_TRAP)) {
                AnimateDeath();
            }
        }

        private void AnimateDeath()
        {
            AudioPlayer.Sfx.Play("blood-spill");
            AudioPlayer.Sfx.Play("explosion");
            GameObject.Instantiate(explodePrefab, position: this.transform.position, rotation: Quaternion.identity);
            Destroy(GetComponentInParent<Route>().gameObject);
        }
    }
}