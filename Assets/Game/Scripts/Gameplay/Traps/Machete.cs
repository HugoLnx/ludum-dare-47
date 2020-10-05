using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Machete : MonoBehaviour
{
    [SerializeField] private Rigidbody2D head;
    [SerializeField] private float height;
    [SerializeField] private float delay;
    [SerializeField] private float fallDuration;
    [SerializeField] private float liftDuration;
    [SerializeField] private Ease fallEase;
    [SerializeField] private Ease liftEase;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private int shakeVibrato;
    [SerializeField] private float shakeRandomness;

    private float delayTime;
    private Sense sense;
    private bool animating;
    private float startY;

    private void Awake() {
        this.delayTime = 99f;
        this.sense = GetComponentInChildren<Sense>();
    }
    
    private void Start() {
        this.startY = this.head.position.y;
    }

    private void Update() {
        if (animating) return;
        this.delayTime += Time.deltaTime;
        if (this.delayTime >= this.delay && sense.IsSensing) {
            StartCoroutine(StartAnimation());
            this.delayTime = 0f;
        }
    }

    private IEnumerator StartAnimation()
    {
        this.animating = true;

        var shakeTween = this.head.transform.DOShakePosition(
            duration: this.shakeDuration,
            strength: this.shakeStrength,
            vibrato: this.shakeVibrato,
            randomness: this.shakeRandomness
        )
        .SetUpdate(UpdateType.Fixed);
        AudioPlayer.Sfx.Play("metal-shake");
        yield return shakeTween.WaitForCompletion();

        var fallTween = this.head.DOMoveY(
            endValue: this.startY - this.height,
            duration: this.fallDuration
        )
        .SetEase(this.fallEase)
        .SetUpdate(UpdateType.Fixed);
        yield return fallTween.WaitForPosition(0.1f);
        AudioPlayer.Sfx.Play("machete-fall");
        yield return fallTween.WaitForCompletion();

        var liftTween = this.head.DOMoveY(
            endValue: this.startY,
            duration: this.liftDuration
        )
        .SetEase(this.liftEase)
        .SetUpdate(UpdateType.Fixed);
        yield return liftTween.WaitForPosition(0.1f);
        AudioPlayer.Sfx.Play("machete-lift");
        yield return liftTween.WaitForCompletion();

        this.animating = false;
    }
}