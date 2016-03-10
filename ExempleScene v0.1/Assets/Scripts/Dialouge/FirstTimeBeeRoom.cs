using UnityEngine;
using System.Collections;

public class FirstTimeBeeRoom : NPC {

    // Use this for initialization
    void OnLevelWasLoaded() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
            SharedVariables.firstTimeBeeRoom = false;
        }
    }
}
