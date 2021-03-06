﻿using UnityEngine;
using System.Collections;

public class Adolf : NPC {

    void Start() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
        DialogueReader.aLinda = GetComponent<Animator>();
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }
    }
}
