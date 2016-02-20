using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{

    public Transform seeker;
    public Movement movement;
    private Vector3 targetPosition;
    public GameObject startingBackground;
    private Animator anim;
    private bool activated = true;
    Grid grid;
    Queue<Vector3> moveQueue = new Queue<Vector3>();
    public Vector3 Target{
        set {
            if (activated) {
                targetPosition = value;
                moveQueue.Clear();
                FindPath(seeker.transform.position, targetPosition);
                if (moveQueue.Count != 0) {
                    movement.Target = moveQueue.Peek();
                }
            }

        }
        get{
            return targetPosition;
        }
    }
    void Start(){
        movement = GetComponent<Movement>();
        anim = GetComponent<Animator>();
    }
    public void setGrid(GameObject newBackground) {
        grid = newBackground.GetComponent<Grid>();
    }

    public Vector3 getTargetNodePosition() {
        Node node = grid.NodeFromWorldPoint(targetPosition);
        return node.position;

    }

    public Vector3 getNodePosition(Vector3 position) {
        Node node = grid.NodeFromWorldPoint(position);
        return node.position;
    }

    void OnSceneWasLoaded(){
        grid = GameObject.Find(startingBackground.name).GetComponent<Grid>();
    }

    public void Update(){
        
        moveSeeker();
    }

    public void setIsActive(bool boolean) {
        activated = boolean;
    }

    public bool getIsActive() {
        return activated;
    }

    void moveSeeker(){
        if (moveQueue.Count != 0){
            if (moveQueue.Peek() == movement.transform.position){
                if (moveQueue.Count != 0){
                    moveQueue.Dequeue();
                    if(moveQueue.Count != 0)
                    movement.Target = moveQueue.Peek();
                }
                    if (movement.transform.position == Target){
                        moveQueue.Clear();
                    }
            }

        }
    }
    public void endPathfinding() {
        anim.SetBool("Walking", false);
        anim.SetFloat("hSpeed", 0);
        anim.SetFloat("vSpeed", 0);
        Target = seeker.position;
        moveQueue.Clear();
    }

    void FindPath(Vector3 startPos, Vector3 targetPos){
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0){
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode){
                RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode)){
                if (!neighbour.walkable || closedSet.Contains(neighbour)){
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parentNode = currentNode;

                    if (!openSet.Contains(neighbour)){
                        openSet.Add(neighbour);
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode){
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode){
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Reverse();
        grid.path = path;
        for (int i = 0; i < path.Count; i++){
            moveQueue.Enqueue(path[i].position);
        }
    }
    int GetDistance(Node nodeA, Node nodeB){
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        else
            return 14 * dstX + 10 * (dstY - dstX);
    }
}
