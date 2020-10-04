using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform cam;
    private float length, startPos;
    [SerializeField] private float spdParallax;
    private SpriteRenderer srenderer;

    private void Awake() {
        this.srenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.transform;
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
        float rePos = (cam.transform.position.x * (1 - spdParallax));
        float dist = (cam.transform.position.x * spdParallax);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if(rePos > startPos + length/2)
        {
            startPos += length;
        }
        else if (rePos < startPos - length/2)
        {
            startPos -= length;
        }
    }
}
