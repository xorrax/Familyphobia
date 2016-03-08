using UnityEngine;
using System.Collections;
public class LightToggle : MonoBehaviour {
    public GameObject flashLightSprite;
    bool räka = true; //Skåda detta majestätiska djur. Löste alla våra problem!

    void Awake() {
        flashLightSprite.SetActive(false);
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
