using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform cam;
    private float length;
    [SerializeField] private float spdParallax;
    private SpriteRenderer srenderer;
    private float offset;

    private void Awake() {
        this.srenderer = GetComponent<SpriteRenderer>();
        this.offset = 0f;
    }
    private void Start()
    {
        cam = Camera.main.transform;
        this.transform.position = new Vector3(cam.position.x, this.transform.position.y, this.transform.position.z);
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        CreateCopy(-length);
        CreateCopy(length);
    }

    private void CreateCopy(float xOffset)
    {
        var copyPosition = this.transform.position + (Vector3.right*xOffset);
        var obj = new GameObject($"{this.name}-copy");
        obj.transform.position = copyPosition;
        obj.transform.SetParent(this.transform);
        var copyrenderer = obj.AddComponent<SpriteRenderer>();
        copyrenderer.sprite = srenderer.sprite;
        copyrenderer.sortingLayerID = srenderer.sortingLayerID;
        copyrenderer.sortingOrder = srenderer.sortingOrder;
        copyrenderer.material = srenderer.material;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.right * ((cam.transform.position.x * (1f-spdParallax)) + this.offset);
        var toCamera = cam.position.x - transform.position.x;

        if (toCamera < -length)
        {
            this.offset -= length;
        }
        else if (toCamera > length)
        {
            this.offset += length;
        }
    }
}
