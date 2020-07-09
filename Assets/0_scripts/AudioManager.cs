using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
//using System.Diagnostics;

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

    public void FadeOutSound(string name)
    {
        Debug.Log("AudioManager::FadeOutSound(string)");

        // Find sound in sounds array where sound.name == name.
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // If no sound found of that name, then return to avoid error.
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        StartCoroutine(AudioManager.StartFade(s.source, 12.0f, 0.0f));
    }

    public void FadeInSound(string name)
    {
        Debug.Log("AudioManager::FadeInSound(string)");

        // Find sound in sounds array where sound.name == name.
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // If no sound found of that name, then return to avoid error.
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        StartCoroutine(AudioManager.StartFade(s.source, 2.0f, 1.0f));
    }

    public void ResetSoundVolume(string name)
    {
        // Find sound in sounds array where sound.name == name.
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // If no sound found of that name, then return to avoid error.
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = 1.0f;
    }


    // https://gamedevbeginner.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/#first_method
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
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
