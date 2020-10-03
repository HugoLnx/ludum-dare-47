using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
            transform.Translate (Vector2.right * speed);
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            transform.Translate (Vector2.right * -speed);
        }
        if(Input.GetKey(KeyCode.DownArrow)){
             transform.Translate (Vector2.up * -speed);
        }
        if(Input.GetKey(KeyCode.UpArrow)){
            transform.Translate (Vector2.up * speed);
        }
    }
}
