using UnityEngine;
using System.Collections;

public class PlaySoundEffect : MonoBehaviour {
    AudioSource audioSource;
    private Animator anim;

    public void playSoundeffect(AudioClip soundEffect) {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffect;
        audioSource.Play();
    }
}
