using UnityEngine;
using System.Collections;

public class RoomLightSwitch : MonoBehaviour {
    RoomLighter roomlighter;

    void Start() {
        roomlighter.GetComponent<RoomLighter>();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (gameObject.name == "EntranceButton") {
                roomlighter.switchEntranceBool();
                print("det är löst");
            } else if (gameObject.name == "KitchenButton") {
                roomlighter.switchKitchenBool();
                print("det är löst");
            } else if (gameObject.name == "LivingButton") {
                roomlighter.switchLivingBool();
                print("det är löst");
            } else if (gameObject.name == "ÖverButton") {
                roomlighter.switchÖverBool();
                print("det är löst");
            }
        }
    }
}
