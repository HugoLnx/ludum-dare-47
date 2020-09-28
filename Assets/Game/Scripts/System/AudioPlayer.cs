using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public enum PlayerType { SFX, CONTINUOUS_SFX, MUSIC }
    [System.Serializable]
    public class AudioGroup
    {
        public string name;
        public AudioSource source;
        public AudioClip[] clips;
        public bool randomPick;
        public bool introLoop;
        private int inx = 0;

        public AudioClip GetClip()
        {
            if (randomPick) {
                return clips[UnityEngine.Random.Range(0, clips.Length)];
            } else {
                var clip = clips[inx];
                inx = (inx + 1) % clips.Length;
                return clip;
            }
        }
    }

    public PlayerType type;
    public bool muted;
    public bool IsMuted => muted || IsMusic;
    public bool IsMusic => type == PlayerType.MUSIC;
    public bool IsSimpleSFX => type == PlayerType.SFX;
    public bool IsContinuousSFX => type == PlayerType.CONTINUOUS_SFX;
    public bool IsSFX => IsSimpleSFX || IsContinuousSFX;
    public bool IsLoopable => IsMusic || IsContinuousSFX;
    public AudioClip[] allClips;

    public AudioGroup[] allGroups;
    private Dictionary<string, AudioClip> clipsByName;
    private Dictionary<string, AudioGroup> groupsByName;
    private Dictionary<string, AudioSource> sourcesByName;
    private Dictionary<string, float> sourcesMaxVolume;
    private static Dictionary<string, AudioPlayer> instances = new Dictionary<string, AudioPlayer>();
    private string lastPlayed;
    private AudioSource source;
    private AudioSource[] allSources;

    public static AudioPlayer GetInstance(string name) => instances[name];
    public static AudioPlayer Sfx => GetInstance("sfx");
    public static AudioPlayer ContinuousSfx => GetInstance("continuous-sfx");
    public static AudioPlayer ContinuousSfx2 => GetInstance("continuous-sfx-2");
    public static AudioPlayer ContinuousSfxBoss => GetInstance("continuous-sfx-boss");
    public static AudioPlayer Music => GetInstance("music");

    public bool IsAnySourcePlaying => allSources.Any(s => s != null && s.isPlaying);

    public const float DEFAULT_VOLUME = 0.75f;
    private float volume = DEFAULT_VOLUME;
    public float Volume {
        get => volume;
        set {
            this.volume = value;
            UpdateSourcesVolume();
        }
    }

    private void UpdateSourcesVolume()
    {
        foreach (var source in allSources) {
            source.volume = sourcesMaxVolume[source.name] * volume;
        }
    }

    private void Awake()
    {
        if (instances.ContainsKey(name)) {
            Destroy(this.gameObject);
            return;
        }
        instances[name] = this;
        this.source = GetComponent<AudioSource>();
        this.allSources = GetComponentsInChildren<AudioSource>();
        instances[name].clipsByName = allClips.Aggregate(new Dictionary<string, AudioClip>(), (dict, c) => { dict.Add(c.name, c); return dict; });
        instances[name].groupsByName = allGroups.Aggregate(new Dictionary<string, AudioGroup>(), (dict, c) => { dict.Add(c.name, c); return dict; });
        this.sourcesByName = allSources.Aggregate(new Dictionary<string, AudioSource>(), (dict, s) => { dict.Add(s.name, s); return dict; });
        this.sourcesMaxVolume = allSources.Aggregate(new Dictionary<string, float>(), (dict, s) => { dict.Add(s.name, s.volume); return dict; });
        gameObject.AddComponent<SceneAwakeListener>().action = OnSceneAwake;
        Volume = volume;
    }

    private void OnSceneAwake(SceneAwakeEvent evt)
    {
        if (IsMusic) return;
        foreach (var src in allSources) {
            if (src.loop) src.Stop();
        }
    }

    public static void UpdateSfxVolume(float volume) {
        Sfx.Volume = volume;
        ContinuousSfx.Volume = volume;
        ContinuousSfx2.Volume = volume;
        ContinuousSfxBoss.Volume = volume;
    }
    public static void UpdateMusicVolume(float volume) {
        Music.Volume = volume;
    }

    public static void StopAllSFX() {
        foreach (var instance in instances.Values) {
            if (instance.IsSFX) instance.Stop();
        }
    }

    public void Play(string name, string overrideSource = null)
    {
        if (IsMuted) return;
        var source = SelectSourceFor(name);
        if (IsAnySourcePlaying && name == lastPlayed && IsLoopable) return;
        this.lastPlayed = name;
        if (IsLoopable) Stop();
        if (IsLoopable && IsIntroLoopClip(name)) {
            var group = groupsByName[name];
            StartCoroutine(PlayIntroLoop(intro: group.clips[0], loop: group.clips[1]));
        } else {
            PlayClip(source, SelectClipFor(name));
        }
    }

    public void Stop()
    {
        this.source.Stop();
        foreach (var source in allSources) {
            source.Stop();
        }
    }

    private IEnumerator PlayIntroLoop(AudioClip intro, AudioClip loop)
    {
        var introSrc = sourcesByName["intro"];
        var loopSrc = sourcesByName["loop"];
        introSrc.clip = intro;
        loopSrc.clip = loop;
        introSrc.Play();
        yield return new WaitUntil(() => introSrc.isPlaying);
        loopSrc.PlayScheduled(AudioSettings.dspTime + intro.length - introSrc.time);
    }

    private bool IsIntroLoopClip(string name)
    {
        if (clipsByName.ContainsKey(name)) return false;
        if (!groupsByName.ContainsKey(name)) return false;
        var group = groupsByName[name];
        return group.introLoop;
    }

    private AudioSource SelectSourceFor(string name)
    {
        if (clipsByName.ContainsKey(name)) return this.source;
        var source = groupsByName[name].source;
        return (source == null ? this.source : source);
    }

    private AudioClip SelectClipFor(string name)
    {
        if (clipsByName.ContainsKey(name)) return clipsByName[name];
        return groupsByName[name].GetClip();
    }

    private void PlayClip(AudioSource source, AudioClip clip)
    {
        if (source == null) return;
        if (IsSimpleSFX) source.PlayOneShot(clip);
        else
        {
            source.clip = clip;
            source.Play();
        }
    }
}
