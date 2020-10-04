using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pendulum : MonoBehaviour
{
    private readonly Vector2 NEUTRAL_DIRECTION = Vector2.down;
    private const float NEUTRAL_ANGLE = 0f;
    [SerializeField] private bool rotateHead;
    [SerializeField] private Transform supportRef;
    [SerializeField] private Rigidbody2D headRef;
    [SerializeField] private float range;
    [SerializeField] private float swingDuration;
    [SerializeField] private Ease swingDownEase;
    [SerializeField] private Ease swingUpEase;
    private float radius;
    private SpriteMask chainMask;
    private float currentAngle;
    private Sequence tween;

    private Vector2 SupportPosition => supportRef.position;
    private Vector2 HeadPosition {
        get => headRef.position;
        set => headRef.position = value;
    }
    private float StartAngle => -range/2f;
    private float EndAngle => range/2f;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable() {
        RestartSwing();
    }

    private void Setup()
    {
        this.radius = (SupportPosition - Mathh.V3to2(headRef.transform.position)).magnitude;
        this.chainMask = supportRef.GetComponentInChildren<SpriteMask>();
        this.chainMask.transform.localScale = new Vector3(1f, this.radius, 1f);
        this.chainMask.transform.position = SupportPosition + (Vector2.down * this.radius/2f);
    }

    [ExposeMethodInEditor]
    private void RestartSwing()
    {
        this.tween?.Kill();
        this.tween = DOTween.Sequence()
        .Append(CreateSwingTween(this.swingDownEase, from: EndAngle, to: NEUTRAL_ANGLE))
        .Append(CreateSwingTween(this.swingUpEase, from: NEUTRAL_ANGLE, to: StartAngle))
        .SetLoops(-1, LoopType.Yoyo)
        .SetUpdate(UpdateType.Fixed)
        .Play();
    }

    private void MoveToAngle(float angle)
    {
        this.currentAngle = angle;
        this.HeadPosition = HeadPositionFor(angle);
        Utils.Set2DRotation(supportRef, -angle);
        if (rotateHead) {
            this.headRef.SetRotation(-angle);
        }
    }

    private Vector2 HeadPositionFor(float angle)
        => SupportPosition + (Mathh.RotateVector(NEUTRAL_DIRECTION, angle, clockwise: true).normalized * radius);

    private Tween CreateSwingTween(Ease ease, float from, float to) =>
        DOTween.To(
            getter: () => this.currentAngle,
            setter: v => this.MoveToAngle(v),
            endValue: to,
            duration: this.swingDuration/2f
        ).From(from).SetEase(ease);

    private void OnDrawGizmos() {
        if (!Application.isPlaying) Setup();
        var angles = new List<float> {
            StartAngle,
            NEUTRAL_ANGLE,
            EndAngle,
        };
        GizmosUtils.OnColor(Color.red, () => {
            foreach(var angle in angles) {
                var headPos = HeadPositionFor(angle);
                Gizmos.DrawLine(SupportPosition, headPos);
                Gizmos.DrawWireSphere(headPos, 0.4f);
            }
        });
    }
}