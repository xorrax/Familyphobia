using UnityEngine;
using System.Collections;

public class FirstTimeDreamworld : NPC {

	// Use this for initialization
    void OnLevelWasLoaded() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;

        if (SharedVariables.FirstTimeDreamworld) {
            interact();
            SharedVariables.FirstTimeDreamworld = false;
        } else {
            Destroy(this);
        }
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }
    }
}
