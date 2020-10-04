using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoWalk : MonoBehaviour
{
    [SerializeField] private float speed;
    private Camera cam;

    private void Awake() {
        this.cam = Camera.main;
    }

    private void Update() {
        this.cam.transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
