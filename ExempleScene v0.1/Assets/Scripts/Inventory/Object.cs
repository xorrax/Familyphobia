using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour{

    public Sprite resultSprite;
    public GameObject combinationObject;
    public GameObject goalObject;
    public Vector3 pathfindingPos;
    public bool mainObject;
    public string comboName;
    public string goalName;
    public float clickDistance;

    //Sound
    public AudioClip lightSound;
    public AudioClip mediumSound;
    public AudioClip heavySound;

    private AudioClip mySound;
    //------

    private Color mouseOverColor = Color.green;
    private Color originalColor;
    private bool myDragging = false;
    private bool otherDragging = false;
    private bool canCombine = false;
    private bool hasCombined = false;
    private Vector3 goalPathfindingPos = Vector3.zero;
    private Vector2 myPos = new Vector2();
    private GameObject player;
    private GameObject otherComboObject;
    private string invName;
    private bool clicked = false;
    private bool mouseOutside = true;
    private bool onGoal = false;
    private float goalDistance = 0f;
    private enum invState{
        DRAGGING,  //..
        INVENTORY, //I inventory
        SLEEPING, //Innan man gjort något med objektet
        COMBINATION, //när man kombinerar två objekt
    };
    private invState otherState = invState.SLEEPING;
    private invState myState = invState.SLEEPING;

    private float myDistance;

	void Start () {
        if (comboName != string.Empty)
            combinationObject = GameObject.Find(comboName);

        if (goalName != string.Empty)
            goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        if (gameObject.tag != "ObjectiveTrigger")
            canCombine = true;

        if (combinationObject == null)
            hasCombined = true;

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponent<Rigidbody2D>().mass == 1)
            mySound = lightSound;
        else if(gameObject.GetComponent<Rigidbody2D>().mass == 2)
            mySound = mediumSound;
        else if(gameObject.GetComponent<Rigidbody2D>().mass == 3)
            mySound = heavySound;

        gameObject.GetComponent<AudioSource>().clip = mySound;

        if(goalObject != null)
            goalObject.SendMessage("SetGoalPathfindingPos", pathfindingPos);
	}
	
	void Update () {

        this.gameObject.GetComponent<Rigidbody2D>().WakeUp();

        if (combinationObject != null)
        {
            if (combinationObject.activeSelf)
            {
                combinationObject.SendMessage("OtherState", (int)myState);
                combinationObject.SendMessage("OtherDragging", myDragging);
            }
        }

        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= clickDistance)
        {
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
            clicked = false;
            Debug.Log(name + " picked up");
        }

        if (onGoal && Vector3.Distance(player.transform.position, goalObject.transform.position) <= goalDistance)
        {
            onGoal = false;
            if (combinationObject == null && otherComboObject == null)
            {
                if (!Input.GetMouseButton(0))
                {
                    myState = invState.COMBINATION;
                    Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                    Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                    this.gameObject.SetActive(false);
                }
            }
            else if (hasCombined)
            {
                if (!Input.GetMouseButton(0))
                {
                    myState = invState.COMBINATION;
                    Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                    Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                    this.gameObject.SetActive(false);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            onGoal = false;

            if(mouseOutside)
                clicked = false;
        }
	}

    void FixedUpdate(){
        if (myDragging && myState != invState.SLEEPING){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnTriggerStay2D(Collider2D col){

        if (combinationObject != null && combinationObject.gameObject.activeSelf && col.gameObject.name == combinationObject.name && canCombine
            && myState != invState.SLEEPING && otherState != invState.SLEEPING)
        {
            myState = invState.COMBINATION;
            if (!myDragging && !otherDragging)
            {
                if (mainObject)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = resultSprite;
                    myState = invState.INVENTORY;
                    hasCombined = true;
                }
                else
                {
                    Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                    Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                    myState = invState.INVENTORY;
                    this.gameObject.SetActive(false);
                }
                myState = invState.INVENTORY;

            }
        }
        else if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name && canCombine)
        {
            if (Vector3.Distance(transform.position, col.transform.position) <= goalDistance)
            {
                if (combinationObject == null && otherComboObject == null)
                {
                    if (!Input.GetMouseButton(0))
                    {
                        myState = invState.COMBINATION;
                        Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                        Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                        this.gameObject.SetActive(false);
                    }
                }
                else if (hasCombined)
                {
                    if (!Input.GetMouseButton(0))
                    {
                        myState = invState.COMBINATION;
                        Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                        Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                        this.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                onGoal = true;
                player.SendMessage("SetTargetPos", goalPathfindingPos);
            }
        }


        if (col.name == invName && myState != invState.INVENTORY){
            myState = invState.INVENTORY;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if (col.name == Inventory.invInstance.gameObject.name){
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
        }
    }


    void SetGoalPathfindingPos(Vector3 pos){
        goalPathfindingPos = pos;
    }

    void SetGoalDistance(float distance)
    {
        goalDistance = distance;
    }

    void OtherState(invState value){
        otherState = value;
    }
    void OtherDragging(bool dragging){
        otherDragging = dragging;
    }

    void OtherCombObject(GameObject otherObject){
        otherComboObject = otherObject;
    }
    void SetPosition(Vector2 pos){
        myPos = pos;
    }

    void CanCombine(bool value){
        canCombine = value;
    }

    void OnMouseEnter(){
        this.gameObject.GetComponent<Renderer>().material.color = mouseOverColor;
        mouseOutside = false;
    }

    void OnMouseExit(){
        this.gameObject.GetComponent<Renderer>().material.color = originalColor;
        mouseOutside = true;
    }

    void OnMouseDown(){
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        myDragging = true;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("HoldingItem", true);
    }

    void OnMouseUp(){
        myDragging = false;

        if (myState == invState.SLEEPING)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= clickDistance)
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
                gameObject.GetComponent<AudioSource>().Play();
                Debug.Log(name + " picked up");
            }
            else
            {
                clicked = true;
                player.SendMessage("SetTargetPos", pathfindingPos);
            }
        }
        else if(myState == invState.COMBINATION){
            StartCoroutine(wait());
        }
        else if(myState == invState.INVENTORY){
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            this.gameObject.transform.position = myPos;
        }
        else if (myState != invState.INVENTORY && myState != invState.COMBINATION){
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;

        player.SendMessage("HoldingItem", false);
        clicked = true;
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}