using UnityEngine;
using System.Collections;

public class MenuSwitcher : MonoBehaviour {
    public GameObject optionsMeny;
    public GameObject mobilMeny;
    public GameObject saveMeny;
    public GameObject loadMeny;
    public GameObject exitMeny;
    public void toggleOptions() {
        optionsMeny.SetActive(true);
        mobilMeny.SetActive(false);
    }

    public void antiToggleOptions() {
        optionsMeny.SetActive(false);
        mobilMeny.SetActive(true);
    }

    public void toggleSave() {
        saveMeny.SetActive(true);
        mobilMeny.SetActive(false);
    }

    public void antiToggleSave() {
        saveMeny.SetActive(false);
        mobilMeny.SetActive(true);
    }

    public void toggleLoad() {
        loadMeny.SetActive(true);
        mobilMeny.SetActive(false);
    }

    public void antiToggleLoad() {
        loadMeny.SetActive(false);
        mobilMeny.SetActive(true);
    }

    public void toggleExit() {
        exitMeny.SetActive(true);
        mobilMeny.SetActive(false);
    }

    public void antiToggleExit() {
        exitMeny.SetActive(false);
        mobilMeny.SetActive(true);
    }
}



