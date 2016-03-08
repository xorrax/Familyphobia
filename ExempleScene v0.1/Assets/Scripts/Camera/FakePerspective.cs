using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FakePerspective : MonoBehaviour {

    Transform thisObject;
    Vector3 startScale;
    float startSpeed;
    float height;
    public float depth;
    public float depthOffset;


	void Start () {
        thisObject = GetComponent<Transform>();
        startScale = thisObject.localScale;
        if (thisObject.GetComponent<Movement>() != null)
        startSpeed = thisObject.gameObject.GetComponent<Movement>().getSpeed();
	}
	void Update () {
        if (SceneManager.GetActiveScene().name != "LoadScene") {
                height = -transform.localPosition.y + depthOffset;
                float newScale = ((height * startScale.y) * depth) * 0.1f;
                float newSpeed = ((height * startSpeed) * depth * 0.8f) * 0.1f;


                thisObject.localScale = new Vector3(newScale, newScale, thisObject.localScale.z);
                if(thisObject.GetComponent<Movement>() != null)
                thisObject.GetComponent<Movement>().setSpeed(newSpeed);
            }
        //}
	}

    public void setStartScale(Vector3 newScale) {
        startScale = newScale;
    }
}
