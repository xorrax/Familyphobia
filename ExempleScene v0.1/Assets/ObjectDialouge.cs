using UnityEngine;
using System.Collections;

public class ObjectDialouge : NPC {

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
	}

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
