using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour {
    public AudioClip soundEffect;
    AudioSource audioSource;

    public void playAudio(AudioClip audioClip) {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
