using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    [SerializeField] private string tag;
    public bool IsSensing {get; private set;}
    
    private void OnTriggerEnter2D(Collider2D other) => OnEnter(other);
    private void OnCollisionEnter2D(Collision2D other) => OnEnter(other.collider);
    private void OnTriggerExit2D(Collider2D other) => OnExit(other);
    private void OnCollisionExit2D(Collision2D other) => OnExit(other.collider);
    private void OnTriggerStay2D(Collider2D other) => OnStay(other);
    private void OnCollisionStay2D(Collision2D other) => OnStay(other.collider);

    private void OnEnter(Collider2D collider)
    {
        if (!collider.CompareTag(tag)) return;
        IsSensing = true;
    }

    private void OnStay(Collider2D collider)
    {
        if (!collider.CompareTag(tag)) return;
        IsSensing = true;
    }

    private void OnExit(Collider2D collider)
    {
        if (!collider.CompareTag(tag)) return;
        IsSensing = false;
    }
}
