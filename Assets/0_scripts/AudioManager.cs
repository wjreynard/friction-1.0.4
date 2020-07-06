using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    // Static reference to current instance of AudioManager in scene
    public static AudioManager instance;

    private void Awake() {

        // We only want there to be one instance of our AudioManager
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        // Let AudioManager persist between scenes
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
    }

    private void Start() {
        PlaySound("Music");
    }

    public void PlaySound(string name) {
        // Find sound in sounds array where sound.name == name.
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // If no sound found of that name, then return to avoid error.
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    //public void PlayRandomSound(string name) {
    //    // Find sound in sounds array where sound.name == name.
    //    Sound s = Array.Find(sounds, sound => sound.name == name);

    //    // If no sound found of that name, then return to avoid error.
    //    if (s == null)
    //    {
    //        Debug.LogWarning("Sound: " + name + " not found!");
    //        return;
    //    }
        
    //    s.source.Play();
    //}
}
