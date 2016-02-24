using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Options : MonoBehaviour {
    Dropdown dropdownMenuResolution;
    void Start() {
        Screen.fullScreen = true;
        dropdownMenuResolution = GameObject.Find("DropdownRes").GetComponent<Dropdown>();
    }

    public void toggleFullscreen() {
        if (Screen.fullScreen == false)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;
    }

    public void setResolutionIngame() {
        if (dropdownMenuResolution.value == 0) {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
        else if (dropdownMenuResolution.value == 1) {
            Screen.SetResolution(1680, 1050, Screen.fullScreen);
        }
        else if (dropdownMenuResolution.value == 2) {
            Screen.SetResolution(1600, 900, Screen.fullScreen);
        }
        else if (dropdownMenuResolution.value == 3) {
            Screen.SetResolution(1440, 900, Screen.fullScreen);
        }
        else if (dropdownMenuResolution.value == 4) {
            Screen.SetResolution(1400, 1050, Screen.fullScreen);
        }
        else if (dropdownMenuResolution.value == 5) {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
        }
    }
}
