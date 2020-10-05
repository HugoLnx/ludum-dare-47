using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadSingleton : MonoBehaviour
{
    private static DontDestroyOnLoadSingleton instance;
    private void Awake() {
        SceneAwakeEvent.Instance.Emit();
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        Debug.Log("PLAY");
        AudioPlayer.Music.Play("main");
    }
}
