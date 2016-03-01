using UnityEngine;
using System.Collections;

public class SpriteOnClick : MonoBehaviour {
    public Sprite nextSprite;
    public Sprite firstSprite;
    SpriteRenderer renderer;
    void Start() {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
        firstSprite = renderer.sprite;
    }

    void spriteOnClick() {
        if (renderer.sprite != nextSprite) {
            renderer.sprite = nextSprite;
        } else {
            renderer.sprite = firstSprite;
        }
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            spriteOnClick();
        }
    }
}