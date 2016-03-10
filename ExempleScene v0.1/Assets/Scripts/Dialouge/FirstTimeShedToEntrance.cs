using UnityEngine;
using System.Collections;

public class FirstTimeShedToEntrance : NPC {

    void OnLevelWasLoaded() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
            SharedVariables.firstTimeShedToEntrance = false;
        }
    }
}
