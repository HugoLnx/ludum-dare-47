using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlade : MonoBehaviour
{
    public static int visibleCount = 0;
    private Rigidbody2D body;
    [SerializeField] private float speed;


    // Start is called before the first frame update
    void Awake()
    {
        this.body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.body.rotation = body.rotation + Time.deltaTime * speed;
    }

    private void OnBecameVisible() {
        visibleCount++;
    }
    private void OnBecameInvisible() {
        visibleCount--;
    }
}
