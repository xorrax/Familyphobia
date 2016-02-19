using UnityEngine;
using System.Collections;

public class FrameTree : MonoBehaviour {

    public Sprite scarfSprite;
    public GameObject scarf;
    public Vector3 pathfindingPos;
    public float goalDistance;

    private bool onGoal = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scarf.SendMessage("SetTreeDistance", goalDistance);        
    }

    void Update()
    {
        if (onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            gameObject.GetComponent<SpriteRenderer>().sprite = scarfSprite;
            GameObject tempObject = GameObject.Find("EmptyFrame");
            tempObject.transform.position = new Vector3(tempObject.transform.position.x, tempObject.transform.position.y, 0);
            tempObject = GameObject.Find("CupCake");
            tempObject.transform.position = new Vector3(tempObject.transform.position.x, tempObject.transform.position.y, 0);
            Debug.Log("Scarf on tree");
        }

        if (Input.GetMouseButtonDown(0))
            onGoal = false;
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.name == "Scarf" && !Input.GetMouseButton(0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
            {
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                gameObject.GetComponent<SpriteRenderer>().sprite = scarfSprite;
                GameObject tempObject = GameObject.Find("EmptyFrame");
                tempObject.transform.position = new Vector3(tempObject.transform.position.x, tempObject.transform.position.y, 0);
                tempObject = GameObject.Find("CupCake");
                tempObject.transform.position = new Vector3(tempObject.transform.position.x, tempObject.transform.position.y, 0);
                Debug.Log("Scarf on tree");
            }
            else
            {
                player.SendMessage("SetTargetPos", pathfindingPos);
                onGoal = true;
            }
        }
    }

    void SetSortingOrder()
    {
        GameObject tempObject = GameObject.Find("EmptyFrame");
        tempObject.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        tempObject = GameObject.Find("CupCake");
        tempObject.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }
}
