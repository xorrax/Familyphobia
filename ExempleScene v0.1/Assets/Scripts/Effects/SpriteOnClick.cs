using UnityEngine;
using System.Collections;

public class SpriteOnClick : MonoBehaviour {
    public Sprite secondSprite;
    Sprite firstSprite;
    SpriteRenderer rendering;
    private AudioSource audioSource;
    public AudioClip soundEffectOpen;
    public AudioClip soundEffectClose;
    private BoxCollider boxCollider;
    void Start() {
        rendering = this.gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = this.gameObject.GetComponent<BoxCollider>();
        firstSprite = rendering.sprite;
    }

    void spriteOnClick() {
        if (rendering.sprite != secondSprite) {
            rendering.sprite = secondSprite;
            boxCollider.center = new Vector3(boxCollider.center.x + 2, boxCollider.center.y, boxCollider.center.z);
            audioSource.clip = soundEffectOpen;
        } else {
            rendering.sprite = firstSprite;
            boxCollider.center = new Vector3(boxCollider.center.x - 2, boxCollider.center.y, boxCollider.center.z);
            audioSource.clip = soundEffectClose;
        }
        audioSource.Play();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            spriteOnClick();
        }
    }
}