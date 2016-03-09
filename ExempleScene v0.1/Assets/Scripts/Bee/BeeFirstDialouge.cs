using UnityEngine;
using System.Collections;

public class BeeFirstDialouge : NPC {
    public GameObject Bee;



    GameObject camera;
    void Start() {
        camera = GameObject.Find("Birthday_Camera");
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
    }

    void Update() {
        if (Player.dialogueObjects.Count > 0) {
            camera.gameObject.GetComponent<BeeCamera>().zoomOut();
            gameObject.GetComponent<Bi>().enabled = true;
            Bee.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override void interact() {

        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }

    }

   
}
