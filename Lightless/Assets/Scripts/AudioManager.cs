using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioMixer masterMixer;
    public float initialMusicVolume;
    public float initialEfxVolume;
    public Sound[] sounds;

    public static AudioManager Instance { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
        }
    }

    void Start() {
        masterMixer.SetFloat("MusicVol", Mathf.Log10(initialMusicVolume) * 20);
        masterMixer.SetFloat("EfxVol", Mathf.Log10(initialEfxVolume) * 20);
        Play("MainTheme");
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            Debug.LogWarning("Sound: " + name + " not found!");
        else {
            if (s.alwaysFinish && s.source.isPlaying)
                return;

            s.source.Play();
        }
    }

    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            Debug.LogWarning("Sound: " + name + " not found!");
        else
            s.source.Stop();
    }

    public void SetMusicLevel(float sliderValue) {
        masterMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

     public void SetEfxLevel(float sliderValue) {
        masterMixer.SetFloat("EfxVol", Mathf.Log10(sliderValue) * 20);
    }

}
