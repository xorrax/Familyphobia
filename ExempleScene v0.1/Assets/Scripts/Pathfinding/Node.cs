using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {
    public bool walkable;
    public Vector3 position;
    public int gCost = 0;
    public int hCost = 0;
    public int gridX = 0;
    public int gridY = 0;
    public Node parentNode;
    int heapIndex;

    public Node(Vector3 nodePosition, bool isWalkable, int nodeGridX, int nodeGridY){
        position = nodePosition;
        walkable = isWalkable;
        gridX = nodeGridX;
        gridY = nodeGridY;
    }

    public int fCost{
        get{
            return gCost + hCost;
        }
    }

    public int HeapIndex{
        get{
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare){
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0){
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
