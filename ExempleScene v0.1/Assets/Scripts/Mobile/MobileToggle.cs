using UnityEngine;
using System.Collections;
public class MobileToggle : MonoBehaviour {
    bool isPaused = false;
    public GameObject optionsMenu;
    void Start() {
        for (int i = 0; i < transform.childCount; ++i) {
            transform.GetChild(i).gameObject.SetActive(isPaused);
        }
    }
    void togglePhone() {
        isPaused = !isPaused;
        transform.GetChild(1).gameObject.SetActive(isPaused);
    }
    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (optionsMenu.active) {
                //cryMeARiver.SetActive(false);
                //togglePhone();
            } else
                togglePhone();
        }
    }
}