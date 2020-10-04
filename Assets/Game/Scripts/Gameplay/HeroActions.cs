using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD47 {
    public class HeroActions : MonoBehaviour
    {
        private const string TAG_TRAP = "Trap";
        private void OnTriggerEnter2D(Collider2D other) => OnCollision(other);
        private void OnTriggerStay2D(Collider2D other) => OnCollision(other);

        private void OnCollision(Collider2D other)
        {
            if (other.CompareTag(TAG_TRAP)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}