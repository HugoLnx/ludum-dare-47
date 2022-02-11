using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LongBlade : MonoBehaviour
{
    private SpriteRenderer srenderer;
    private Rigidbody2D body;
    private Sense sense;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private int shakeVibrato;
    [SerializeField] private float shakeRandomness;
    [SerializeField] private float delay;
    [SerializeField] private float chopAngle;
    [SerializeField] private float chopDuration;
    [SerializeField] private float liftDuration;
    [SerializeField] private Ease chopEase;
    [SerializeField] private Ease liftEase;
    private float initialRotation;

    private void Awake() {
       this.srenderer = GetComponentInChildren<SpriteRenderer>();
       this.body = GetComponent<Rigidbody2D>();
       this.sense = GetComponentInChildren<Sense>();
    }

    private void Start() {
       this.initialRotation = this.body.rotation;
    }

    private void OnEnable() {
       StartCoroutine(WaitToChop());
    }

    private IEnumerator WaitToChop()
    {
       while (true) {
           yield return new WaitUntil(() => sense.IsSensing);
           yield return AnimatedChop();
           yield return new WaitForSeconds(this.delay);
       }
    }

    private IEnumerator AnimatedChop()
    {
       AudioPlayer.Sfx.Play("metal-shake");
       var shakeTween = this.srenderer.transform.DOShakePosition(
           duration: this.shakeDuration,
           strength: this.shakeStrength,
           vibrato: this.shakeVibrato,
           randomness: this.shakeRandomness
       );

       var chopTween = this.body.DORotate(
           endValue: this.initialRotation + this.chopAngle,
           duration: this.chopDuration
       )
       .SetEase(this.chopEase)
       .OnPlay(() => AudioPlayer.Sfx.Play("long-blade-chop"));

       var liftTween = this.body.DORotate(
           endValue: this.initialRotation,
           duration: this.liftDuration
       ).SetEase(this.liftEase);

       var tween = DOTween.Sequence()
           .Append(shakeTween)
           .Append(chopTween)
           .Append(liftTween)
           .SetUpdate(UpdateType.Fixed)
           .Play();

       yield return tween.WaitForCompletion();
    }

}