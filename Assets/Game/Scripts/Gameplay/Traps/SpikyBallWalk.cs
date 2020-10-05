using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikyBallWalk : MonoBehaviour
{
    [SerializeField] private float chainLength;
    [SerializeField] private float speed;
    [SerializeField] private Transform support1;
    [SerializeField] private Transform support2;
    private Rigidbody2D trap;
    private SpriteMask mask;
    private float ChainOrigin => -chainLength/2f;
    private float ChainEnd => chainLength/2f;

    private void Awake() {
        Setup();
        this.trap = GetComponentInChildren<Rigidbody2D>();
    }

    private void OnEnable() {
        StartWalkThroughChain();
    }

    private void StartWalkThroughChain()
    {
        var p = this.trap.transform.localPosition;
        this.trap.transform.localPosition = new Vector3(p.x, ChainOrigin, p.z);
        this.trap.transform.DOLocalMoveY(
            endValue: ChainEnd,
            duration: chainLength/speed
        )
        .From(ChainOrigin)
        .SetLoops(-1, LoopType.Yoyo)
        .SetUpdate(UpdateType.Fixed)
        .SetEase(Ease.Linear);
    }

    private void OnDrawGizmos() {
        if (Application.isPlaying) return;
        Setup();
    }

    private void Setup()
    {
        this.mask = GetComponentInChildren<SpriteMask>();
        var l = this.mask.transform.localScale;
        this.mask.transform.localScale = new Vector3(l.x, chainLength, l.z);
        Utils.SetLocalPosition(support1, y: ChainOrigin);
        Utils.SetLocalPosition(support2, y: ChainEnd);
    }
}