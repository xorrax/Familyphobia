using UnityEngine;
using System.Collections;

public class Shanon : NPC {

   /* public string newScene;
    public string newRoom;
    bool oneClick = true;
    const float MIN_TIME = 0.00f;
    const float MAX_TIME = 2f;
    float time = 0;*/

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

    void OnMouseOver() {
       /* if (Input.GetMouseButtonDown(0)) {

            if (!oneClick && time < MAX_TIME && time > MIN_TIME) {
                SharedVariables.NewRoom = newRoom;
                if (!GameObject.Find("WarpFade")) {

                   // audioSource.Play();

                }
               
            }
            if (oneClick) {
                oneClick = false;
            }
        }
        time += Time.deltaTime;
        if (time > MAX_TIME) {
            oneClick = true;
            time = 0;
        }*/
    }

   /* void OnMouseExit() {
        oneClick = true;
        time = 0;
    }*/

    void FixedUpdate() {

    }

}
