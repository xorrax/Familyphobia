using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bouquet : MonoBehaviour {

    List<GameObject> flowerList = new List<GameObject>();

    public GameObject goalObject;
    public string goalName;


    private float goalDistance;
    private Vector3 goalPathfindingPos;
    private Color mouseOverColor = Color.green;
    private Color originalColor;

    private bool onGoal = false;
    private bool myDragging = false;
    private bool canCombine = true;
    private bool sentFlowerAmount = false;
    private bool clickMove = false;
    private float clickTimer = 0f;
    private Vector3 myPos = Vector2.zero;
    private GameObject player;
    private GameObject otherComboObject;

    static public Bouquet bouquetInstance;

    private bool addedToInv = false;
    
    private enum invState{
        DRAGGING,  //..
        INVENTORY, //I inventory
        SLEEPING, //Innan man gjort något med objektet
        COMBINATION, //när man kombinerar två objekt
    };
    private invState otherState = invState.SLEEPING;
    private invState myState = invState.SLEEPING;

    private float myDistance;

	void Start (){
        bouquetInstance = this;
        if (goalName != string.Empty)
            goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        player = GameObject.FindGameObjectWithTag("Player");
        
	}
	
	void Update () {
        if (onGoal && Vector3.Distance(player.transform.position, goalObject.transform.position) < goalDistance)
        {
            if(flowerList.Count < 3)
                Inventory.invInstance.SendMessage("SetPositions");
        }

        if (Input.GetMouseButtonDown(0))
        {
            onGoal = false;
        }

        if (flowerList.Count == 3 && !sentFlowerAmount)
        {
            sentFlowerAmount = true;
            goalObject.SendMessage("GotFlowers");
        }
    }

    void FixedUpdate(){

        if (clickMove)
        {
            clickTimer += Time.deltaTime;
            if (clickTimer >= 1)
            {
                clickMove = false;
                clickTimer = 0;
            }
        }

        if (myDragging && myState != invState.SLEEPING){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnTriggerStay(Collider col){
        if (!Input.GetMouseButton(0) && goalObject != null && col.gameObject.name == goalObject.name && canCombine)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < goalDistance)
            {
                myState = invState.COMBINATION;

                if (flowerList.Count < 3)
                    Inventory.invInstance.SendMessage("SetPositions");
            }
            else
            {
                player.SendMessage("SetTargetPos", goalPathfindingPos);
                onGoal = true;
            }
        }


        if (col.name == "Inventory" && myState != invState.INVENTORY)
        {
            myState = invState.INVENTORY;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Inventory")
        {
            Debug.Log("exiting inv");
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
        }
    }
    void SetPosition(Vector3 pos){
        myPos = pos;
    }

    void CanCombine(){
        canCombine = true;
    }

    void SetGoalDistance(float dist)
    {
        goalDistance = dist;
    }

    void SetGoalPathfindingPos(Vector3 pos)
    {
        goalPathfindingPos = pos;
    }

    void OnMouseEnter(){
        this.gameObject.GetComponent<Renderer>().material.color = mouseOverColor;
    }

    void OnMouseExit(){
        this.gameObject.GetComponent<Renderer>().material.color = originalColor;
    }

    void OnMouseDown(){
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        if(!myDragging)
        {
            clickMove = true;
            myDragging = true;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("CanWalk", false);
    }

    void OnMouseUp(){
        if(clickMove)
        {
            clickMove = false;
            clickTimer = 0f;
            myDragging = true;
        }
        else
        {
            myDragging = false;

            if (myState == invState.INVENTORY)
            {
                this.gameObject.transform.position = myPos;
                Inventory.invInstance.SendMessage("SetPositions");
                player.SendMessage("CanWalk", true);

            }
            else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
            {
                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;

                player.SendMessage("CanWalk", true);
            }
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
            player.SendMessage("CanWalk", true);
        }
    }
    void AddFlower(GameObject flower){

        if (!addedToInv){
            Inventory.invInstance.SendMessage("AddItem", gameObject);
            addedToInv = true;
            myState = invState.INVENTORY;
        }


        flowerList.Add(flower);
        flower.transform.position = gameObject.transform.position;    
    }

    void RemoveBouquet(){
        Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
        Inventory.invInstance.SendMessage("SetPositions");
        for (int i = 0; i < flowerList.Count; i++)
        {
            flowerList[i].SendMessage("Remove");
        }
        this.gameObject.SetActive(false);
    }
}




    
