using UnityEngine;
using System.Collections;

public class Stol : MonoBehaviour {

    public GameObject cupcake;
    public GameObject linda;
    public Sprite sprite;

    public float goalDistance;
    public Vector3 pathfindingPos;

    private GameObject player;
    private bool onGoal = false;

    void Start(){
        cupcake.SendMessage("SetGoalDistance", goalDistance);
        player = GameObject.Find("Jack");
    }

    void Update()
    {
        if(onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            linda.SendMessage("IsDistracted", true);
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            player.SendMessage("CanWalk", true);
        }

        if (Input.GetMouseButtonDown(0))
            onGoal = false;
    }

    void OnTriggerStay(Collider col){

        if (col.name == cupcake.name && !Input.GetMouseButton(0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
            {
                linda.SendMessage("IsDistracted", true);
                gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                player.SendMessage("CanWalk", true);
            }
            else
            {
                player.SendMessage("SetTargetPos", pathfindingPos);
                onGoal = true;
            }
        }
    }
}
