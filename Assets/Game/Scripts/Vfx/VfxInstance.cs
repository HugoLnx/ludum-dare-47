using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxInstance : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        this.animator = GetComponentInChildren<Animator>();
    }

    public void Play(string name, Vector2 position) {
        this.transform.position = position;
        this.animator.Play(name);
    }
}
