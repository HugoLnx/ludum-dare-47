using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private bool pressedRight;
    private bool pressedLeft;
    private bool pressedUp;
    private bool pressedDown;

    private void Awake() {
        this.body = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        this.pressedRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        this.pressedLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        this.pressedUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        this.pressedDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }

    private void FixedUpdate()
    {
        if (this.pressedRight) {
            body.MovePosition(body.position + (Vector2.right * speed * Time.fixedDeltaTime));
        }
        if (this.pressedLeft) {
            body.MovePosition(body.position + (Vector2.left * speed * Time.fixedDeltaTime));
        }
        if (this.pressedUp) {
            body.MovePosition(body.position + (Vector2.up * speed * Time.fixedDeltaTime));
        }
        if (this.pressedDown) {
            body.MovePosition(body.position + (Vector2.down * speed * Time.fixedDeltaTime));
        }
        this.pressedRight = false;
        this.pressedLeft = false;
        this.pressedUp = false;
        this.pressedDown = false;
    }
}
