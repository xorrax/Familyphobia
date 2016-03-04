using UnityEngine;
using System.Collections;

public class EyeMovement : MonoBehaviour {
    Transform jack;
    SpriteRenderer lawnmower;
    SpriteRenderer background;
    Vector3 lawnmowerMin;
    Vector3 lawnmowerMax;
    Vector3 backgroundMin;
    Vector3 backgroundMax;
    Vector3 realMin;
    Vector3 realMax;
    float procentX;

    void Start() {
        jack = GameObject.Find("Jack").GetComponent<Transform>();
        background = GameObject.Find("Entrance_Background").GetComponent<SpriteRenderer>();
        lawnmower = transform.GetComponentInParent<SpriteRenderer>();
        lawnmowerMin = lawnmower.bounds.min;
        lawnmowerMax = lawnmower.bounds.max;
        backgroundMin = background.bounds.min;
        backgroundMax = background.bounds.max;

        procentX = ((lawnmowerMax.x - lawnmowerMin.x) / (backgroundMax.x - backgroundMin.x)) * 4;
    }
    void eyeXMovement() {
        transform.position = new Vector3(jack.transform.position.x * procentX, 0, -1);
    }

	void Update () {
        eyeXMovement();
	}
}
