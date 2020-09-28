using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class AutoMove : MonoBehaviour
{
    [SerializeField] private float moveX;
    [SerializeField] private Ease easeX;
    [SerializeField] private float durationX;
    [SerializeField] private float moveY;
    [SerializeField] private Ease easeY;
    [SerializeField] private float durationY;
    private Vector3 originalPosition;
    private Tween tween;

    private void Awake() {
        this.originalPosition = this.transform.position;
    }
    private void OnEnable() {
        RestartAnimation();
    }

    private void OnDisable() {
        KillTween();
    }

    [ExposeMethodInEditor]
    private void RestartAnimation()
    {
        KillTween();
        var xTween = this.transform.DOMoveX(this.originalPosition.x + this.moveX, this.durationX)
        .SetEase(this.easeX);

        var yTween1 = this.transform.DOMoveY(this.originalPosition.y + this.moveY, this.durationY/4f)
        .SetEase(this.easeY);

        var yTween2 = this.transform.DOMoveY(this.originalPosition.y - (this.moveY*2f), this.durationY/4f*2f)
        .SetEase(this.easeY);

        var yTween3 = this.transform.DOMoveY(this.originalPosition.y, this.durationY/4f)
        .SetEase(this.easeY);

        var yTween = DOTween.Sequence()
        .Append(yTween1)
        .Append(yTween2)
        .Append(yTween3)
        .Play();

        this.tween = DOTween.Sequence()
        .Join(xTween)
        .Join(yTween)
        .SetLoops(-1, LoopType.Yoyo)
        .Play();
    }

    private void KillTween()
    {
        if (this.tween != null) {
            this.tween.Rewind();
            this.tween.Kill();
        }
    }
}
