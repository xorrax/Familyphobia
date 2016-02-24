using UnityEngine;
using System.Collections;

public class FlowerDialogue : NPC {
    public GameObject secondDialogue;

    void Start() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }
    }

    void setDialogue() {
        Debug.Log("Change dialogue");
        gameObject.GetComponent<DialogueReader>().dialogueIn = secondDialogue;
    }

}
