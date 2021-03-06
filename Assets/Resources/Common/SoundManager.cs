using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundManager : MonoBehaviour {
    [System.Serializable]
    public struct Sound {
        public string name;
        public AudioClip[] clip;
    }

    public Sound[] audioList = { };
    private Dictionary<string, AudioClip[]> audios = new Dictionary<string, AudioClip[]>();

    public AudioSource music;
    public static SoundManager instance = null;
    public AudioSource[] allAudioSources;

    //private void Awake() {
    //    // If there is not already an instance of SoundManager, set it to this.
    //    if (instance == null) {
    //        instance = this;
    //    }
    //    //If an instance already exists, destroy whatever this object is to enforce the singleton.
    //    else if (instance != this) {
    //        Destroy(gameObject);
    //    }

    //    //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
    //    DontDestroyOnLoad(gameObject);
    //}

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        foreach (Sound s in audioList) {
            audios.Add(s.name, s.clip);
        }

        allAudioSources = FindObjectsOfType<AudioSource>();

        PlayMusic("Background");
    }

    // Play a random audioclip from the specified name options in the given audioSource. Can be played with delay
    public void Play(string name, AudioSource audioSource, float timeDelay = 0f) {
        if (audioSource != null && audios.ContainsKey(name)) {
            audioSource.clip = audios[name][Random.Range(0, audios[name].Length)];
            audioSource.PlayDelayed(timeDelay);
        }
    }

    // Play a random audioclip from the specified name options in the given position
    public void PlayAndDestroy(string name, Vector3 position, float volume = 1f) {
        if (audios.ContainsKey(name)) {
            AudioSource.PlayClipAtPoint(audios[name][Random.Range(0, audios[name].Length)], position, volume);
        }
    }

    public void PlayMusic(string name) {
        music.clip = audios[name][0];
        music.Play();
    }

    public void Pause(bool pause) {
        foreach (AudioSource audioSource in allAudioSources) {
            if (audioSource != music) {
                if (pause) {
                    audioSource.Pause();
                } else {
                    audioSource.UnPause();
                }
            }
        }
    }

}

