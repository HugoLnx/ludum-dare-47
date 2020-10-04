using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject MainCamera;
    private float length, startPos;
    [SerializeField] private float spdParallax;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (MainCamera.transform.position.x * (1 - spdParallax));
        float dist = (MainCamera.transform.position.x * spdParallax);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if(temp > startPos + length/2)
        {
            startPos += length;
        }
        else if (temp < startPos - length/2)
        {
            startPos -= length;
        }
    }
}
