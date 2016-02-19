using UnityEngine;
using System.Collections;



public class NPC : MonoBehaviour {
    public string newScene;
    public string newRoom;

    bool oneClick = true;
    const float MIN_TIME = 0.00f;
    const float MAX_TIME = 2f;
    float time = 0;
    public NPC self;

    public warpToScene warp;

    public virtual void interact() {

    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
           
            if (!oneClick && time < MAX_TIME && time > MIN_TIME) {
                SharedVariables.NewRoom = newRoom;
                warp.LoadScene(newScene);
            }
            if (oneClick) {
                oneClick = false;
            }
        }
        time += Time.deltaTime;
        if (time > MAX_TIME) {
            oneClick = true;
            time = 0;
        }
    }
    
    void OnMouseExit() {
        oneClick = true;
        time = 0;
    }

}
