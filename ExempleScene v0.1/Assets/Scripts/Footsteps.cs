using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {
    public AudioClip soundEffect;
    public Vector2 pitchRange = new Vector2(-1, 1);
    AudioSource audioSource;
    private Animator anim;

    public void playFootsteps() {
        audioSource = GetComponent<AudioSource>();
        float pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.pitch = pitch;
        audioSource.clip = soundEffect;
        audioSource.Play();
    }

}
