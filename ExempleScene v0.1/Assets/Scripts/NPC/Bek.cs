using UnityEngine;
using System.Collections;

public class Bek : NPC {

    void Start() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
        DialogueReader.aBek = GetComponent<Animator>();
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }
    }
}
