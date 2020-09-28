using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesVfxInstance : MonoBehaviour, IParticleVfxInstance
{
    public float noiseDistance = 0f;
    private ParticleSystem master;
    private Dictionary<string, ParticleSystem> activableParticles = new Dictionary<string, ParticleSystem>();

    private void Awake() {
        this.master = GetComponent<ParticleSystem>();
        var particles = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
        foreach (var particle in particles) {
            if (particle.gameObject.activeSelf) continue;
            activableParticles[particle.name] = particle;
        }
        this.gameObject.AddComponent<SceneAwakeListener>().action = OnSceneAwake;
    }

    private void OnSceneAwake(SceneAwakeEvent evt)
    {
        master.Stop();
    }

    public void Play(Vector2? position = null, float? noise = null, float? rotation = 0f, bool flipX = false, float duration=0f, float scale = 1f) {
        var realPosition = position.HasValue  ? position.Value : Mathh.V3to2(this.transform.position);
        var noiseDistance = noise.HasValue ? noise.Value : this.noiseDistance;
        var xNoise = noiseDistance * UnityEngine.Random.value - noiseDistance/2f;
        var yNoise = noiseDistance * UnityEngine.Random.value - noiseDistance/2f;
        this.transform.position = realPosition + new Vector2(xNoise, yNoise);
        if (rotation.HasValue) this.transform.rotation = Quaternion.Euler(0f, 0f, rotation.Value);
        this.transform.localScale = new Vector3(scale, scale, 1f);
        if (flipX) {
            var s = this.transform.localScale;
            this.transform.localScale = new Vector3(s.x*-1f, s.y, s.z);
        }
        master.Play();
        if (duration > 0f) StartCoroutine(StopIn(duration));
    }

    private IEnumerator StopIn(float delay)
    {
        yield return new WaitForSeconds(delay);
        master.Stop();
    }

    public void Activate(string name) {
        activableParticles[name].gameObject.SetActive(true);
    }

    public void Deactivate(string name) {
        activableParticles[name].gameObject.SetActive(false);
    }

    public void Reset() {
        foreach (var particle in activableParticles.Values) {
            particle.gameObject.SetActive(false);
        }
    }
}
