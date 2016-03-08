using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bek : NPC {

    CameraMovement thisCamera;
    List<Transform> spawnPoints;

    Player player;
    FakePerspective thisPerspective;

    void Start() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
        DialogueReader.aBek = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        thisPerspective = transform.parent.GetComponent<FakePerspective>();
        updateSpawnPoints();
        
    }

    public override void interact() {
        if (gameObject.GetComponent<DialogueReader>() != null) {
            gameObject.GetComponent<DialogueReader>().enabled = true;
        }
    }

    void Update() {
        if (thisPerspective != player.GetComponent<FakePerspective>()) {
            thisPerspective.depth = player.GetComponent<FakePerspective>().depth;
            thisPerspective.depthOffset = player.GetComponent<FakePerspective>().depthOffset;
        }
        if(Camera.main != null)
        thisCamera = Camera.main.GetComponent<CameraMovement>();


    }

    public void updateSpawnPoints() {
        spawnPoints = new List<Transform>();
        foreach (Transform child in GameObject.Find(player.currentRoom + "_BekSpawn").transform) {
            spawnPoints.Add(child);
        }

        transform.parent.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    }

    public Vector3 getNewPosition() {
        return transform.parent.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    }
}
