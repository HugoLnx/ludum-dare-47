using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWalk : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake() {
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        this.rb.MovePosition(this.rb.position + Vector2.right * 5f * Time.fixedDeltaTime);
    }
}
