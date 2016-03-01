using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour {
    public AudioClip currentClip;
    private AudioSource audioSource;
    private AudioClip lastClip;
    private float lastClipTime;
    private float fadeValue = 0.005f;
    private float volume;
    private bool coroutineRunning = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = currentClip;
        lastClip = currentClip;
        lastClipTime = 0;
        audioSource.Play();
    }

    public void changeAudio(AudioClip newAudio, float time) {
        if (newAudio != audioSource.clip) {
            object[] parms = new object[2]{newAudio, time};
            if (!coroutineRunning) {
                volume = audioSource.volume;
            }
            StartCoroutine("MusicFadeOut", parms);
        }
    }

    IEnumerator MusicFadeOut(object[] parms) {
        coroutineRunning = true;
        while(audioSource.volume >= 0){
            audioSource.volume -= fadeValue;

            if (audioSource.volume <= 0) {
                lastClip = audioSource.clip;
                lastClipTime = audioSource.time;

                audioSource.Stop();

                currentClip = (AudioClip)parms[0];

                audioSource.clip = currentClip;
                audioSource.time = (float)parms[1];
                StartCoroutine("MusicFadeIn");
                StopCoroutine("MusicFadeOut");
                
            }
            yield return null;
        }
    }

    IEnumerator MusicFadeIn() {
        while (audioSource.volume <= volume) {
            audioSource.volume += fadeValue;
            yield return null;
        }
        coroutineRunning = false;
    }
    public void playLastClip() {
        changeAudio(lastClip, lastClipTime);
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
