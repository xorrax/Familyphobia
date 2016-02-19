using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class RenderQue : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        float temp = -transform.position.y * 100;
        spriteRenderer.sortingOrder = (int)temp;
    }
}


