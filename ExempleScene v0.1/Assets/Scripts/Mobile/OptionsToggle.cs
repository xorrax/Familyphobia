using UnityEngine;
using System.Collections;

public class OptionsToggle : MonoBehaviour {
    public GameObject optionsMeny;
    public GameObject mobilMeny;
    public void toggleOptions() {
        optionsMeny.SetActive(true);
        mobilMeny.SetActive(false);
    }

    public void antiToggleOptions() {
        optionsMeny.SetActive(false);
        mobilMeny.SetActive(true);
    }
}


