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
    //public AudioSource effect;
    public static SoundManager instance = null;

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
    }

    private void Start() {
        foreach (Sound s in audioList) {
            audios.Add(s.name, s.clip);
        }
        
        PlayMusic("Background");
    }

    public void Play(string name, AudioSource audioSource, float time = 0f) {
        if (audioSource != null && audios.ContainsKey(name)) {
            audioSource.clip = audios[name][Random.Range(0, audios[name].Length - 1)];
            audioSource.PlayDelayed(time);
        }
    }

    public void PlayMusic(string name) {
        music.clip = audios[name][0];
        music.Play();
    }
}

