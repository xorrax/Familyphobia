using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RepeatingClouds : MonoBehaviour {
    private float startPosition;
    public float speed =0.5f;
    private float width;

    private List<Transform> clouds; 

    void Start() {
        clouds = new List<Transform>();
        foreach (Transform child in transform) {
            width = child.gameObject.GetComponent<Renderer>().bounds.size.x;
            clouds.Add(child);
        }
        startPosition = clouds[0].position.x - (width * 1);
        for (int i = 0; i < clouds.Count; i++) {
            clouds[i].position = new Vector2(startPosition + (width * i), 0);
        }

        



        
    }

    void FixedUpdate() {
        foreach (Transform cloud in clouds) {
            cloud.transform.position = new Vector3(cloud.transform.position.x + speed, 0, cloud.transform.position.z);
            if (cloud.position.x >= startPosition + (width * clouds.Count)) {
                cloud.position = new Vector2(startPosition, 0);
            }
        }
    }


}
