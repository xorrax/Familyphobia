using UnityEngine;
using System.Collections;

public class EyeMovement : MonoBehaviour {
    Transform jack;
    SpriteRenderer lawnmower;
    SpriteRenderer background;
    Vector3 lawnmowerMin;
    Vector3 lawnmowerMax;
    Vector2 lawnmowerDistance;
    Vector3 backgroundMin;
    Vector3 backgroundMax;
    Vector2 backgroundDistance;
    Vector2 lawnAndBackDifference;
    float procentX;

    void Start() {
        jack = GameObject.Find("Jack").GetComponent<Transform>();
        background = GameObject.Find("Entrance_Background").GetComponent<SpriteRenderer>();
        lawnmower = transform.GetComponentInParent<SpriteRenderer>();
        lawnmowerMin = lawnmower.bounds.min;
        lawnmowerMax = lawnmower.bounds.max;
        lawnmowerDistance = new Vector2(lawnmowerMax.x - lawnmowerMin.x, lawnmowerMax.y - lawnmowerMin.y);
        backgroundMin = background.bounds.min;
        backgroundMax = background.bounds.max;
        backgroundDistance = new Vector2(backgroundMax.x - backgroundMin.x, backgroundMax.y - backgroundMin.y);
        lawnAndBackDifference = new Vector2(lawnmowerDistance.x / backgroundDistance.x, lawnmowerDistance.y / backgroundDistance.y);
        procentX = (lawnAndBackDifference.x + lawnAndBackDifference.y) / 2;
    }

    void eyeMovement() {
        transform.position = new Vector3(transform.parent.position.x + jack.position.x * procentX, transform.parent.position.y + jack.position.y * procentX, -1);
    }

	void Update () {
        eyeMovement();
	}
}
