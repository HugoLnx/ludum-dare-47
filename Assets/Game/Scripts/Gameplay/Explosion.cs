using UnityEngine;
using System;
public class Explosion : MonoBehaviour {
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    public static void Explode(Rigidbody2D body, float minAngle, float maxAngle, float minForce, float maxForce) {
        var degrees = UnityEngine.Random.Range(minAngle, maxAngle);
        var force = UnityEngine.Random.Range(minForce, maxForce);
        var radians = degrees * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * force;
        body.AddForce(direction);
    }
}