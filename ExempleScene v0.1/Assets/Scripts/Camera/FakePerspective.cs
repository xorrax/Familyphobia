using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FakePerspective : MonoBehaviour {

    Transform thisObject;
    Vector3 startScale;
    float startSpeed;
    float height;
    public float depth;
    float maxY;
    float minY;


	void Start () {
        thisObject = GetComponent<Transform>();
        startScale = thisObject.localScale;
        startSpeed = thisObject.gameObject.GetComponent<Movement>().getSpeed();
	}
	void Update () {
        if (SceneManager.GetActiveScene().name != "LoadScene") {
            if (Camera.main != null) {
                minY = Camera.main.GetComponent<CameraMovement>().getMinY();
                maxY = Camera.main.GetComponent<CameraMovement>().getMaxY();
                height = maxY - transform.position.y;
                float newScale = ((height * startScale.y) * depth) * 0.1f;
                float newSpeed = ((height * startSpeed) * depth * 0.8f) * 0.1f;


                thisObject.localScale = new Vector3(newScale, newScale, thisObject.localScale.z);
                thisObject.GetComponent<Movement>().setSpeed(newSpeed);
            }
        }
	}

    public void setStartScale(Vector3 newScale) {
        startScale = newScale;
    }
}
