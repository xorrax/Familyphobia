using UnityEngine;
using System.Collections;

public class Bee : MonoBehaviour {

    GameObject camera;
	void Start () {
        camera = GameObject.Find("Birthday_Camera");
	}

	void Update () {
	
	}

    void OnMouseOver() {
        if (Input.GetMouseButton(0)) {
            camera.gameObject.GetComponent<BeeCamera>().zoomOut();
        }
    }
}
