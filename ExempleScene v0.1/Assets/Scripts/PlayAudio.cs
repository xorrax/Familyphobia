using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour {
    public AudioClip soundEffect;
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect;
        audioSource.Play();
    }

}
