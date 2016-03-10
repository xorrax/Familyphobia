using UnityEngine;
using System.Collections;

public class Stol : MonoBehaviour {

    public Sprite sprite;
    public string key;
    public string cupcake;


    public float goalDistance;
    public Vector3 pathfindingPos;

    private GameObject player;
    private bool onGoal = false;

    void Start(){
        player = GameObject.Find("Jack");
    }

    void Update()
    {
        if(onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            GameObject.Find(key).SendMessage("LindaDistracted", true);
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            player.SendMessage("CanWalk", true);
        }

        if (Input.GetMouseButtonDown(0))
            onGoal = false;
    }

    void OnTriggerStay(Collider col){

<<<<<<< HEAD
        if(col.name == cupcake)
            GameObject.Find(cupcake).SendMessage("SetGoalDistance", goalDistance);

=======
>>>>>>> origin/master
        if (col.name == cupcake && Input.GetMouseButton(0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
            {
                GameObject.Find(key).SendMessage("LindaDistracted", true);
                gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                player.SendMessage("CanWalk", true);
            }
            else
            {
<<<<<<< HEAD

=======
>>>>>>> origin/master
                player.SendMessage("SetTargetPos", pathfindingPos);
                onGoal = true;
            }
        }
        else if(col.name == cupcake && Input.GetMouseButtonUp(0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
            {
                GameObject.Find(key).SendMessage("LindaDistracted", true);
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
