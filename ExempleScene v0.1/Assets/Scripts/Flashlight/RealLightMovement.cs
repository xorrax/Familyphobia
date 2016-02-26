using UnityEngine;
using System.Collections;

public class RealLightMovement : MonoBehaviour {
    public GameObject realLightSprite;
    public bool lightToggleCheck = false;
    public Vector2 distanceModifier;
    public float depthSpeed;
    public Vector3 standardPosition;
    float zMovement;
    public GameObject Jack;

    void realLightDirection() {
        if (Camera.main != null) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
            transform.rotation = rot;
            transform.eulerAngles = new Vector3(-transform.eulerAngles.x, -transform.eulerAngles.y, 0);
        }
    }

    void realLightDepth() {
        //if (Camera.main != null) {
        //    Vector3 mousePositionRelative = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        //    if (mousePositionRelative.x > distanceModifier.x || -mousePositionRelative.x < -distanceModifier.x || mousePositionRelative.y > distanceModifier.y || -mousePositionRelative.y < distanceModifier.y) {
        //        if (transform.position.z > standardPosition.z) {
        //            transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y + 3.4f, transform.position.z + mousePositionRelative.z);
        //        }
        //    } else if (mousePositionRelative.x < distanceModifier.x || -mousePositionRelative.x > -distanceModifier.x || mousePositionRelative.y < distanceModifier.y || -mousePositionRelative.y > -distanceModifier.y) {
        //        if (transform.position.z < standardPosition.z) {
        //            transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y + 3.4f, transform.position.z - mousePositionRelative.z);
        //        }
        //    }
        //}
    }

    void FixedUpdate() {
        realLightDirection();
        realLightDepth();
    }
}