using UnityEngine;
using System.Collections;

public class ChangeMesh : MonoBehaviour {
    public Mesh newMesh;
    private Mesh oldMesh;
    private MeshCollider meshCollider;
    private Pathfinding pathfinding;
    private Grid grid;

    void Start() {
        meshCollider = GetComponent<MeshCollider>();
        pathfinding = GameObject.FindGameObjectWithTag("Player").GetComponent<Pathfinding>();
        grid = GetComponent<Grid>();
        changeMesh();
    }
    public void changeMesh() {
        meshCollider.sharedMesh = newMesh;
        grid.createGrid();
        if (pathfinding.getGridBackground() == gameObject.name) {
            pathfinding.setGrid(gameObject);
        }
    }
}
