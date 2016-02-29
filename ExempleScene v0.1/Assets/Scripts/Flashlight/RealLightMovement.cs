using UnityEngine;
using System.Collections;

public class RealLightMovement : MonoBehaviour {
    public GameObject realLightSprite;
    public bool lightToggleCheck = false;
    public float offSet;
    public GameObject Jack;
    Movement movement;
    //////public float speed;

    void realLightDirection() {
        if (Camera.main != null) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Quaternion rotate = Quaternion.LookRotation(transform.position - mousePosition, Vector3.up);
            transform.localRotation = rotate;
            transform.eulerAngles = new Vector3(-transform.eulerAngles.x, -transform.eulerAngles.y, 0);
            if (mousePosition.x > Jack.transform.position.x + offSet) {
                Vector3 mousePositionTwo = new Vector3(mousePosition.x + 5, mousePosition.y, mousePosition.z);
                Quaternion rotateTwo = Quaternion.LookRotation(transform.position - mousePositionTwo, Vector3.up);
                Quaternion slerp = Quaternion.Slerp(rotate, rotateTwo, 5f);
                transform.localRotation = slerp;
                transform.eulerAngles = new Vector3(-transform.eulerAngles.x, -transform.eulerAngles.y, 0);
            } 
            else if (mousePosition.x < Jack.transform.position.x - offSet) {
                Vector3 mousePositionThree = new Vector3(mousePosition.x - 5, mousePosition.y, mousePosition.z);
                Quaternion rotateThree = Quaternion.LookRotation(transform.position - mousePositionThree, Vector3.up);
                Quaternion slerpTwo = Quaternion.Slerp(rotate, rotateThree, 5f);
                transform.localRotation = slerpTwo;
                transform.eulerAngles = new Vector3(-transform.eulerAngles.x, -transform.eulerAngles.y, 0);
            }
        }
    }

    void realLightDepth() {
        if (Camera.main != null) {
            Vector3 mousePosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (mousePosition.x > Jack.transform.position.x + offSet || mousePosition.x < Jack.transform.position.x - offSet) {
                transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y, -3.8f + Mathf.Abs(mousePosition.x));
                if (transform.position.z >= -3.8f){
                    transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y, -3.8f);
                }
            }
            else if (mousePosition.x < Jack.transform.position.x + offSet || mousePosition.x > -Jack.transform.position.x - offSet) {
                transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y, -6.7f);
                if (transform.position.z <= -6.7){
                    transform.position = new Vector3(Jack.transform.position.x, Jack.transform.position.y, -6.7f);
                }
            }
        }
    }

    void Update() {
        realLightDirection();
        realLightDepth();
    }
}