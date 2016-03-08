using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bek : NPC {

    CameraMovement thisCamera;
    List<Transform> spawnPoints;
    GameObject bek;
    Player player;
    FakePerspective thisPerspective;
    bool readyToMove = false;

    void Start() {
        gameObject.AddComponent<NPC>();
        gameObject.GetComponent<NPC>().self = this;
        DialogueReader.aBek = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        bek = transform.parent.gameObject;
        thisPerspective = transform.parent.GetComponent<FakePerspective>();
        spawnPoints = new List<Transform>();
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

        if (!thisCamera.isSeenByCamera(bek)) {
            if (readyToMove)
                bek.transform.position = getNewPosition();
        } else
            readyToMove = true;


    }

    public void updateSpawnPoints() {
        spawnPoints.Clear();
        foreach (Transform child in GameObject.Find(player.currentRoom + "_BekSpawn").transform) {
            spawnPoints.Add(child);
        }

        transform.parent.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    }

    public Vector3 getNewPosition() {
        List<Transform> templist;
        templist = new List<Transform>();
        foreach(Transform child in spawnPoints){
            if (!thisCamera.isSeenByCamera(bek, child.position)) {
                templist.Add(child);
            }
        }

        readyToMove = false;
        player.pathfinding.refreshGrid();
        return templist[Random.Range(0, templist.Count)].position;
    }
}
