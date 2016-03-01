using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour {
    public AudioClip currentClip;
    private AudioSource audioSource;
    private AudioClip lastClip;
    private float lastClipTime;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = currentClip;
        lastClip = currentClip;
        lastClipTime = 0;
        audioSource.Play();
    }

    public void changeAudio(AudioClip newAudio, float time) {
        if (newAudio != audioSource.clip) {
            lastClip = audioSource.clip;
            lastClipTime = audioSource.time;

            audioSource.Stop();

            currentClip = newAudio;

            audioSource.clip = newAudio;
            audioSource.Play();
            audioSource.time = time;
        }
    }

    public AudioClip getCurrentClip() {
        return audioSource.clip;
    }
    public AudioClip getLastClip() {
        return lastClip;
    }
    public float getLastClipTime() {
        return lastClipTime;
    }

    public float getCurrentClipTime() {
        return audioSource.time;
    }

}
