using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LawnMowerDIalouge : NPC{

    public GameObject[] lawnmowerDialouge = new GameObject[2];
    int index;
    bool once = false;
	// Use this for initialization
	void Start () {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
        index = 0;

	}
    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            
            gameObject.GetComponent<DialogueReader>().enabled = true;
            if (index < 1) {
                index++;
            }
        }
        gameObject.GetComponent<DialogueReader>().dialogueIn = lawnmowerDialouge[index];
           
    }
	// Update is called once per frame
	void Update () {
	
	}
}
