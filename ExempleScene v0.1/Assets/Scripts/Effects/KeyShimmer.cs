using UnityEngine;
using System.Collections;

public class KeyShimmer : MonoBehaviour {
    public Transform glimmer;
    public Vector3 glimmerMaxSize;
    public Vector3 glimmerGrowthSpeed;
    bool sizeCheck = false;

    void FixedUpdate() {
        if (sizeCheck == true) {
            glimmer.localScale -= glimmerGrowthSpeed * Time.deltaTime;
            if (glimmer.localScale.x <= 0) {
                sizeCheck = false;
            }
        } else {
            glimmer.localScale += glimmerGrowthSpeed * Time.deltaTime;
            if (glimmer.localScale.x >= glimmerMaxSize.x || glimmer.localScale == glimmerMaxSize) {
                sizeCheck = true;
            }
        }

    }
}