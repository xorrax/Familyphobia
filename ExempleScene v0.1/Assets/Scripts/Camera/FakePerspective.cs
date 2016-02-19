using UnityEngine;
using System.Collections;

public class FakePerspective : MonoBehaviour {

    Transform thisObject;
    Vector3 startScale;
    float height;
    public float depth;
    float maxY;
    float minY;


	void Start () {
        thisObject = GetComponent<Transform>();
        startScale = thisObject.localScale;
	}
	void Update () {
        minY = Camera.main.GetComponent<CameraMovement>().getMinY();
        maxY = Camera.main.GetComponent<CameraMovement>().getMaxY();
        height = maxY - transform.position.y;
        float newScale = ((height * startScale.y) * depth) * 0.1f;


        thisObject.localScale = new Vector3(newScale, newScale, thisObject.localScale.z);
	}
}
