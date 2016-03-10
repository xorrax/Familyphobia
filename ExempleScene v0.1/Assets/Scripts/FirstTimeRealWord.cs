using UnityEngine;
using System.Collections;

public class FirstTimeRealWord : NPC {

    void OnLevelWasLoaded() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;

        if (!SharedVariables.FirstTimeDreamworld && SharedVariables.firstTimeRealWorld) {
            interact();
            SharedVariables.firstTimeRealWorld = false;
        } else {
        }
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }
    }
}
