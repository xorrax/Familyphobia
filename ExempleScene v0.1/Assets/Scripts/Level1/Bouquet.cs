using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bouquet : MonoBehaviour {

    List<GameObject> flowerList = new List<GameObject>();

    public GameObject goalObject;
    public float goalDistance;
    public Vector3 pathfindingPos;
    public string goalName;

    private Color mouseOverColor = Color.green;
    private Color originalColor;

    private bool onGoal = false;
    private bool myDragging = false;
    private bool canCombine = true;
    private bool sentFlowerAmount = false;
    private Vector2 myPos = Vector2.zero;
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

        this.gameObject.GetComponent<Rigidbody2D>().WakeUp();

        if (onGoal && Vector3.Distance(player.transform.position, goalObject.transform.position) < goalDistance)
        {
            goalObject.SendMessage("Bouquet", gameObject);
        }

        if (Input.GetMouseButtonDown(0))
        {
            onGoal = false;
        }
	}

    void FixedUpdate(){
        if (myDragging && myState != invState.SLEEPING){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
        if (flowerList.Count == 4 && !sentFlowerAmount)
        {
            sentFlowerAmount = true;
            goalObject.SendMessage("GotFlowers");
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (!Input.GetMouseButton(0) && goalObject != null && col.gameObject.name == goalObject.name && canCombine)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < goalDistance)
            {
                myState = invState.COMBINATION;

                if (flowerList.Count < 4)
                    Inventory.invInstance.SendMessage("SetPositions");
            }
            else
            {
                player.SendMessage("SetTargetPos", pathfindingPos);
                onGoal = true;
            }
        }


        if (col.name == "Inventory" && myState != invState.INVENTORY)
        {
            myState = invState.INVENTORY;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log(col.name + " exit inv");
        if (col.name == "Inventory")
        {
            Debug.Log("exiting inv");
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
        }
    }
    void SetPosition(Vector2 pos){
        myPos = pos;
    }

    void CanCombine(){
        canCombine = true;
    }

    void OnMouseEnter(){
        this.gameObject.GetComponent<Renderer>().material.color = mouseOverColor;
    }

    void OnMouseExit(){
        this.gameObject.GetComponent<Renderer>().material.color = originalColor;
    }

    void OnMouseDown(){
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        myDragging = true;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("HoldingItem", true);
    }

    void OnMouseUp(){
        myDragging = false;

        if(myState == invState.INVENTORY){
            this.gameObject.transform.position = myPos;
            Inventory.invInstance.SendMessage("SetPositions");
            player.SendMessage("HoldingItem", true);

        }
        else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;

            player.SendMessage("HoldingItem", true);
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
        player.SendMessage("HoldingItem", true);
    }
    void AddFlower(GameObject flower){

        if (!addedToInv){
            Inventory.invInstance.SendMessage("AddItem", gameObject);
            addedToInv = true;
            myState = invState.INVENTORY;
        }


        flowerList.Add(flower);
        flower.transform.position = gameObject.transform.position;


        //Ta inte bort detta~~
        //if (flower.name == "BlueFlower")
        //    flower.SendMessage("ChangeSprite", flower.GetComponent<SpriteRenderer>().sprite = blueFlower);
        //else if (flower.name == "RedFlower")
        //    flower.SendMessage("ChangeSprite", flower.GetComponent<SpriteRenderer>().sprite = redFlower);
        //else if (flower.name == "OrangeFlower")
        //    flower.SendMessage("ChangeSprite", flower.GetComponent<SpriteRenderer>().sprite = orangeFlower);
        //else if (flower.name == "BrownFlower")
        //    flower.SendMessage("ChangeSprite", flower.GetComponent<SpriteRenderer>().sprite = brownFlower);            
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




    
