using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RepeatingClouds : MonoBehaviour {
    public Vector2 maxPosition;

    private List<Transform> clouds; 

    void Start() {
        clouds = new List<Transform>();
        foreach (Transform child in transform) {
            clouds.Add(child);
        }
    }

    void FixedUpdate() {
        foreach (Transform cloud in clouds) {
            cloud.transform.position = new Vector3(cloud.transform.position.x + 0.5f, cloud.transform.position.y, cloud.transform.position.z);
        }
    }


}
