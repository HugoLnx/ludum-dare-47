using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PressStartScene : MonoBehaviour
{
    private bool acceptKey;
    [SerializeField] private Image cover;

    private IEnumerator Start() {
        yield return new WaitForSeconds(1f);
        this.acceptKey = true;
    }

    private void Update() {
        if (acceptKey && Input.anyKeyDown) {
            acceptKey = false;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        AudioPlayer.Sfx.Play("press-start");
        var tween = DOTween.ToAlpha(
            getter: () => cover.color,
            setter: c => cover.color = c,
            endValue: 1f,
            duration: 2f
        ).SetEase(Ease.OutSine).Play();
        yield return tween.WaitForCompletion();
        SceneManager.LoadScene("MainScene");
    }
}