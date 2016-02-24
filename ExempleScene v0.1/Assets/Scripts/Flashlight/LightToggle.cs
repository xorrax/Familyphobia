using UnityEngine;
using System.Collections;
public class LightToggle : MonoBehaviour {
    public GameObject flashLightSprite;
    bool räka = false; //Skåda detta majestätiska djur. Löste alla våra problem!

    void Start() {
        flashLightSprite = GameObject.Find("Flashlight");
    }
    public void toggleLight() {
        if (räka == false) {
            flashLightSprite.SetActive(true);
            räka = !räka;
        } else {
            flashLightSprite.SetActive(false);
            räka = !räka;
        }
    }
}
