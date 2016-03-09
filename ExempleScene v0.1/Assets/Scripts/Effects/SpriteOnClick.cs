using UnityEngine;
using System.Collections;

public class SpriteOnClick : MonoBehaviour {
    public Sprite nextSprite;
    public Sprite firstSprite;
    SpriteRenderer renderer;
    private AudioSource audioSource;
    public AudioClip soundEffectOpen;
    private BoxCollider boxCollider;
    //public AudioClip soundEffectClose;
    void Start() {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = this.gameObject.GetComponent<BoxCollider>();
        firstSprite = renderer.sprite;
    }

    void spriteOnClick() {
        if (renderer.sprite != nextSprite) {
            renderer.sprite = nextSprite;
            boxCollider.center = new Vector3(boxCollider.center.x + 2, boxCollider.center.y, boxCollider.center.z);
            //audioSource.clip = soundEffectOpen;
        } else {
            renderer.sprite = firstSprite;
            boxCollider.center = new Vector3(boxCollider.center.x - 2, boxCollider.center.y, boxCollider.center.z);
            //audioSource.clip = soundEffectClose;
        }
        audioSource.clip = soundEffectOpen;
        audioSource.Play();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            spriteOnClick();
        }
    }
}