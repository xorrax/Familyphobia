using UnityEngine;
using System.Collections;

public class Lawnmower : MonoBehaviour {
    ChangeMesh changeMesh;
    GameObject grass;


    void Start(){
        changeMesh = GameObject.Find("Shed_Background").GetComponent<ChangeMesh>();
        grass = GameObject.Find("Shed_Grass");

	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            changeMesh.changeMesh();
            grass.SetActive(false);
        }
    }
}
