using UnityEngine;
using System.Collections;

public class RealLightMovement : MonoBehaviour {
    public GameObject realLightSprite;
    public bool lightToggleCheck = false;
    public float offSet;
    public GameObject Jack;
    public float speed;

    void realLightDirection() {
        if (Camera.main != null) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.up);
            //transform.localRotation = rot;
            //transform.eulerAngles = new Vector3(-transform.eulerAngles.x, -transform.eulerAngles.y, 0);
            ////transform.localRotation = Quaternion.LookRotation(mousePosition);
            //////Plane playerPlane = new Plane(Vector3.up, transform.position);
            //////Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //////float hitdist = 0.0f;
            //////if (playerPlane.Raycast (ray, out hitdist)) {
            //////    Vector3 targetPoint = ray.GetPoint(hitdist);
            //////    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            //////    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            //////}
        }
    }

    void realLightDepth() {
        if (Camera.main != null) {
            Vector3 mousePosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (mousePosition.x > Jack.transform.position.x + offSet || mousePosition.x < Jack.transform.position.x - offSet) {
                transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y, -6.7f + Mathf.Abs((Jack.transform.position.x - mousePosition.x)));
                if (transform.position.z >= 0) {
                    transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y, -0.10f);
                }
            } 
            else if (mousePosition.x > Jack.transform.position.x + offSet || -mousePosition.x < -Jack.transform.position.x - offSet) {
                if (transform.position.z <= -6.7) {
                    transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y, -6.7f);
                }
            }
        }
    }

    void FixedUpdate() {
        realLightDirection();
        realLightDepth();
    }
}