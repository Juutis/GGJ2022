using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MusicPlayer: MonoBehaviour
{
    public static MusicPlayer main;

    private AudioSource dayMusic;
    private AudioSource nightMusic;

    [SerializeField]
    private AudioClip dayMusicClip;
    [SerializeField]
    private AudioClip nightMusicClip;

    private List<AudioFade> fades = new List<AudioFade>();

    [SerializeField]
    float musicVolume = 0.5f;
    [SerializeField]
    float crossfadeDurationOut = 2.5f;
    [SerializeField]
    float crossfadeDurationIn = 2.5f;

    private bool isCurrentlyDay = true;

    private void Awake() {
        main = this;
    }

    private void Start() {
        Debug.Log("init audios");
        InitializeAudioSources();
        StartMusic(true);
    }

    public void StartMusic(bool isDay) {
        if (isDay) {
            dayMusic.volume = musicVolume;
        } else {
            nightMusic.volume = musicVolume;
        }
        dayMusic.Play();
        nightMusic.Play();
    }

    public void SwitchMusic(bool isDay) {
        if (isDay == isCurrentlyDay) {
            return;
        }
        isCurrentlyDay = isDay;
        string dayOrNight = isDay ? "day" : "night";
        Debug.Log($"Switching to {dayOrNight}");
        if (isDay) {
            CrossFade(nightMusic, dayMusic, crossfadeDurationOut, crossfadeDurationIn);
        } else {
            CrossFade(dayMusic, nightMusic, crossfadeDurationOut, crossfadeDurationIn);
        }
    }


    private void InitializeAudioSources() {
        if (dayMusic == null) {
            dayMusic = InitializeAudioSource("Day music", dayMusicClip);
        }
        if (nightMusic == null) {
            nightMusic = InitializeAudioSource("Night music", nightMusicClip);
        }
    }

    private AudioSource InitializeAudioSource(string name, AudioClip clip) {
        AudioSource source = Prefabs.Get<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.transform.SetParent(transform);
        source.transform.position = Vector2.zero;
        source.loop = true;
        source.name = name;
        return source;
    }

    public void Fade(AudioSource fadeSource, float targetVolume, float duration = 0.5f) {
        AudioFade fade = new AudioFade(duration, targetVolume, fadeSource);
        fades.Add(fade);
    }

    public void CrossFade(AudioSource fadeOutSource, AudioSource fadeInSource, float durationOut, float durationIn) {
        AudioFade fadeOut = new AudioFade(durationOut, 0f, fadeOutSource);
        AudioFade fadeIn = new AudioFade(durationIn, musicVolume, fadeInSource);
        fades.Add(fadeOut);
        fades.Add(fadeIn);
    }

    public void Update() {
        for(int index = 0; index < fades.Count; index += 1) {
            AudioFade fade = fades[index];
            if (fade != null && fade.IsFading) {
                fade.Update();
            }
            if (!fade.IsFading) {
                fades.Remove(fade);
            }
        }
    }
}

public class AudioFade {
    public AudioFade(float duration, float target, AudioSource track) {
        this.duration = duration;
        IsFading = true;
        timer = 0f;
        originalVolume = track.volume;
        targetVolume = target;
        audioSource = track;
    }
    public bool IsFading {get; private set;}
    private float duration;
    private float timer;
    private float targetVolume;
    private AudioSource audioSource;
    private float originalVolume;

    public void Update() {
        timer += Time.unscaledDeltaTime / duration;
        audioSource.volume = Mathf.Lerp(originalVolume, targetVolume, timer);
        if (timer >= 1) {
            audioSource.volume = targetVolume;
            IsFading = false;
        }
    }
}