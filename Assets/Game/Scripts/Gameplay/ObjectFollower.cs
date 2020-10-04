using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Awake() {
        Utils.SetPosition(this.transform, y: 0f);
    }
    private IEnumerator Start() {
        yield return new WaitForFixedUpdate();
        Utils.SetPosition(this.transform, y: 0f);
    }
    private void LateUpdate() {
        Utils.SetPosition(this.transform, x: target.position.x);
    }
    private void FixedUpdate() {
        Utils.SetPosition(this.transform, x: target.position.x);
    }
}
