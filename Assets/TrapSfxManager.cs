using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSfxManager : MonoBehaviour
{
    [SerializeField] private float bladeSfxDelay;

    private void OnEnable() {
        StartCoroutine(RotateBladeSfx());
        StartCoroutine(PendulumSfx());
        StartCoroutine(WalkSpikySfx());
    }

    private IEnumerator WalkSpikySfx()
    {
        yield return new WaitForSeconds(0.5f);
        while (true) {
            yield return new WaitUntil(() => WalkSpikyVisibleCounter.visibleCount > 0);
            AudioPlayer.ContinuousSfx2.Play("walk-spiky-chain");
            yield return new WaitUntil(() => WalkSpikyVisibleCounter.visibleCount <= 0);
            AudioPlayer.ContinuousSfx2.Stop();
        }
    }

    private IEnumerator PendulumSfx()
    {
        yield return new WaitForSeconds(0.5f);
        while (true) {
            yield return new WaitUntil(() => Pendulum.bladeSfx);
            AudioPlayer.Sfx.Play("pendulum-blade");
            yield return new WaitForSeconds(0.5f);
            Pendulum.bladeSfx = false;
        }
    }

    private IEnumerator RotateBladeSfx()
    {
        yield return new WaitForSeconds(0.5f);
        while (true) {
            yield return new WaitUntil(() => BladeVisibleCounter.visibleCount > 0);
            AudioPlayer.Sfx.Play("blade");
            yield return new WaitForSeconds(this.bladeSfxDelay);
            AudioPlayer.Sfx.Play("blade2");
            yield return new WaitForSeconds(this.bladeSfxDelay);
        }
    }
}