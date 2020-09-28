using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsOnDestroy : MonoBehaviour
{
    public string sfx;
    public ParticlesVfxInstance particlesPrefab;
    public Transform particlesTarget;
    public Vector2 particlesOffset;
    private Transform ParticlesTarget => particlesTarget == null ? this.transform : particlesTarget;
    private Vector2 ParticlesPosition => Mathh.V3to2(ParticlesTarget.position) + particlesOffset;
    public UnityEvent onDestroy;
    private bool deactivated;

    private void OnDestroy() {
        if (deactivated) return;
        deactivated = true;
        if (this.onDestroy != null) this.onDestroy.Invoke();
        if (particlesPrefab != null) {
            var vfx = ParticlesVfxInstancePool.Next(particlesPrefab.name);
            if (vfx != null) vfx.Play(ParticlesPosition, rotation: ParticlesTarget.rotation.z);
        }
        if (sfx != "") AudioPlayer.Sfx.Play(sfx);
    }

    public void Deactivate() {
        this.deactivated = true;
    }

    public static void DeactivateOn(GameObject obj)
    {
        var effects = obj.GetComponentInChildren<EffectsOnDestroy>();
        effects.Deactivate();
    }
}
