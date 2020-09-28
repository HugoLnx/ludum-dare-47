using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParticleVfxInstance
{
    void Play(Vector2? position = null, float? noise = null, float? rotation = 0f, bool flipX = false, float duration = 0f, float scale = 1f);
}
