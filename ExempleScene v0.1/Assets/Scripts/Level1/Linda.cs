using UnityEngine;
using System.Collections;

public class Linda : NPC {

    private bool isDistraced = false;
    public GameObject key;

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

    void IsDistracted(bool value){
        isDistraced = value;
        key.SendMessage("LindaDistracted", value);
    }

    void HasWorm(){
        //Linda säger att hon inte gillar mask och bara söta saker?
    }
}
