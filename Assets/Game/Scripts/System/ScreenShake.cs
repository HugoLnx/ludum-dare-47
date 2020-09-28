using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource impulse;
    private float Sustain {
        get => this.impulse.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime;
        set => this.impulse.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = value;
    }

    private void Awake() {
        this.impulse = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float duration = -1f) {
        var oldSustain = Sustain;
        if (duration >= 0f) {
            Sustain = duration;
        }
        this.impulse.GenerateImpulse();
        Sustain = oldSustain;
    }
}
