using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
    public Vector2 gridWorldSize;
    public Node[,] grid;
    BoxCollider boxCollider;
    bool walkable;
    public List<Node> path = new List<Node>();
    public Vector2 gridBoxSize = new Vector2(0.3f, 0.3f);

    int gridSizeX, gridSizeY;
    void OnLevelWasLoaded(){
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / gridBoxSize.x);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / gridBoxSize.y);
        createGrid();
    }

    public int MaxSize{
        get{
            return gridSizeX * gridSizeY;
        }

    }

    public void createGrid(){
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = new Vector3(transform.position.x - (gridWorldSize.x / 2), transform.position.y - ( gridWorldSize.y / 2 ));
        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = new Vector3(bottomLeft.x + (x * gridBoxSize.x), bottomLeft.y + (y * gridBoxSize.y), 0);
                Vector3 castOrigin = new Vector3(worldPoint.x, worldPoint.y, -2);
                Ray ray = new Ray(castOrigin, Vector3.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)){
                    walkable = false;
                    if (hit.collider.tag == "floor"){
                        walkable = true;
                    }
                    if (hit.collider.tag == "Player"){
                        walkable = true;
                    }
                    if (hit.collider.tag == "warp") {
                        walkable = true;
                    }
                }
                else{
                    walkable = false;
                }
               
                grid[x, y] = new Node(new Vector3(worldPoint.x + (gridBoxSize.x / 2), worldPoint.y + (gridBoxSize.y / 2), worldPoint.z), walkable, x, y);
            }

        }
    }

    public List<Node> GetNeighbours(Node node){
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++){
            for (int y = -1; y <= 1; y++){
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition){

        float percentX = ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x) - (transform.position.x / gridWorldSize.x);
        float percentY = ((worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y) - (transform.position.y / gridWorldSize.y);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


   void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        if (grid != null){
            foreach (Node n in grid){
                Gizmos.color = (n.walkable) ? Color.green : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.position, new Vector3(gridBoxSize.x, gridBoxSize.y, 1));
            }
        }
    }
}
