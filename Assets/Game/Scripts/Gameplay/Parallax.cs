using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    private float length, startPos;
    [SerializeField] private float spdParallax;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
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
