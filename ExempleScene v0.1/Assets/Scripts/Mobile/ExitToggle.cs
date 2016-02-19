using UnityEngine;
using System.Collections;
public class ExitToggle : MonoBehaviour {
    public void toggleExitExitButton() {
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void toggleExitNoButton() {
        transform.parent.parent.gameObject.SetActive(false);
    }
}
