using UnityEngine;
using System.Collections;

public class RealLightMovement : MonoBehaviour {
    public GameObject realLightSprite;
    public bool lightToggleCheck = false;

    void realLightDirection() {
        if (Camera.main != null) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
            transform.rotation = rot;
            transform.eulerAngles = new Vector3(-transform.eulerAngles.x, -transform.eulerAngles.y, 0);
            float input = Input.GetAxis("Vertical");
        }
    }

    void FixedUpdate() {
        realLightDirection();
    }
}


