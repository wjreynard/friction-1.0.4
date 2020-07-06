using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip[] audioArray;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void RandomPlay() {
        audioSource.clip = audioArray[Random.Range(0, audioArray.Length)];
        audioSource.PlayOneShot(audioSource.clip);
	}
}
