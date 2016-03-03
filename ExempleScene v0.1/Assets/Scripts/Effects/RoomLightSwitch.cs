using UnityEngine;
using System.Collections;

public class RoomLightSwitch : MonoBehaviour {
    RoomLighter roomlighter;

    void Start() {
        roomlighter = this.gameObject.GetComponentInParent<RoomLighter>();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (gameObject.name == "EntranceButton") {
                roomlighter.switchEntranceBool();
            } else if (gameObject.name == "KitchenButton") {
                roomlighter.switchKitchenBool();
            } else if (gameObject.name == "LivingButton") {
                roomlighter.switchLivingBool();
            } else if (gameObject.name == "ÖverButton") {
                roomlighter.switchÖverBool();
            }
        }
    }
}
