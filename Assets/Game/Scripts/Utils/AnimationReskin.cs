using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class AnimationReskin : MonoBehaviour
{
    private SpriteRenderer srenderer;
    private Dictionary<string, Sprite> sprites;
    public string bundleName;
    private bool isReady = false;

    void Start()
    {
        this.srenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable() {
        StartCoroutine(WaitBundleBeingReady());
    }

    private IEnumerator WaitBundleBeingReady()
    {
        yield return new WaitUntil(() => AssetBundlersManager.Instance.IsReady(bundleName));
        UpdateSprites();
    }

    void LateUpdate()
    {
        if (!this.isReady || srenderer.sprite == null) return;
        var spriteName = srenderer.sprite.name;
        if (this.sprites.ContainsKey(spriteName))
        {
            this.srenderer.sprite = this.sprites[spriteName];
        }
    }

    public void SetSkin(string bundleName)
    {
        this.bundleName = bundleName;
        if (this.enabled && this.gameObject.activeSelf) StartCoroutine(WaitBundleBeingReady());
    }

    private void UpdateSprites()
    {
        this.sprites = new Dictionary<string, Sprite>();
        foreach (var obj in AssetBundlersManager.Instance.GetAll(bundleName))
        {
            var asset = obj as Sprite;
            if (asset != null)
            {
                sprites.Add(asset.name, asset);
            }
        }
        this.isReady = true;
    }
}