using UnityEngine;
using System.Collections;

public class AttackRange : MonoBehaviour {

    public bool inRange = false;

	void OnTriggerEnter(Collider col) {
        if(col.name == "Jack") {
            inRange = true;
        }
    }
    void OnTriggerExit(Collider col) {
        if(col.name == "Jack") {
            inRange = false;
        }
    }
}
