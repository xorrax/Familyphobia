using UnityEngine;
using System.Collections;
public class LightToggle : MonoBehaviour {
    public GameObject flashLight;
    bool räka = false; //Skåda detta majestätiska djur. Löste alla våra problem!

    public void toggleLight() {
        if (räka == false) {
            flashLight.SetActive(true);
            räka = !räka;
        } else {
            flashLight.SetActive(false);
            räka = !räka;
        }
    }
}

