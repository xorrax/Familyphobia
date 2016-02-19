using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
    public GameObject combinationObject;
    public GameObject goalObject;
    public GameObject linda;

    public Vector3 pathfindingPos;
    public float goalDistance;
    public bool mainObject;
    public string comboName;
    public string goalName;

    private Color mouseOverColor = Color.green;
    private Color originalColor;

    private bool myDragging = false;
    private bool otherDragging = false;
    private bool canCombine = false;
    private bool hasCombined = false;
    private bool hasWorm = false;
    private Vector2 myPos = new Vector2();
    private Vector3 goalPathfindingPos = Vector3.zero;
    private GameObject player;
    private GameObject otherComboObject;
    private string invName;
    private bool pickedUp = false;
    private bool lindaDistracted = false;
    private bool lastLindaDistracted;
    private bool onGoal = false;
    private enum invState
    {
        DRAGGING,  //..
        INVENTORY, //I inventory
        SLEEPING, //Innan man gjort något med objektet
        COMBINATION, //när man kombinerar två objekt
    };

    private invState myState = invState.SLEEPING;

    private float myDistance;

    void Start()
    {
        if (comboName != string.Empty)
            combinationObject = GameObject.Find(comboName);

        if (goalName != string.Empty)
            goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        if (gameObject.tag != "CombinationTrigger")
            canCombine = true;

        player = GameObject.FindGameObjectWithTag("Player");

        if (combinationObject == null)
            hasCombined = true;
        linda = GameObject.Find("Linda");
        lastLindaDistracted = !lindaDistracted;
    }

    void Update()
    {

        this.gameObject.GetComponent<Rigidbody2D>().WakeUp();

        if (combinationObject != null)
        {
            if (combinationObject.activeSelf)
            {
                combinationObject.SendMessage("OtherDragging", myDragging);
            }
        }
        if (lindaDistracted != lastLindaDistracted)
        {
            combinationObject.SendMessage("LindaDistracted", lindaDistracted);
            lastLindaDistracted = lindaDistracted;
        }

        if(Vector3.Distance(player.transform.position, goalObject.transform.position) <= goalDistance && onGoal)
        {
            onGoal = false;
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
            this.gameObject.SetActive(false);
            //Gå till bi-minipussel
        }

        if (Input.GetMouseButtonDown(0))
            onGoal = false;
    }

    void FixedUpdate()
    {
        if (myDragging && myState != invState.SLEEPING && pickedUp)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (combinationObject != null && col.gameObject.name == combinationObject.name && !Input.GetMouseButton(0))
        {
            if (hasWorm){
                linda.SendMessage("HasWorm");
            }
            else if(!hasWorm && lindaDistracted){
                Inventory.invInstance.SendMessage("AddItem", gameObject);
                pickedUp = true;
                myState = invState.INVENTORY;
            }
        }
        else if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name && canCombine)
        {
            if (Vector3.Distance(transform.position, col.transform.position) < 3.5f)
            {
                if (combinationObject == null && otherComboObject == null)
                {
                    myState = invState.COMBINATION;
                    if (!myDragging)
                    {
                        Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                        Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                        this.gameObject.SetActive(false);
                        //DO stuff!!
                        //!     Skriv kod här.. gå till bi pussel
                        //! 
                    }
                }
            }
            else
                player.SendMessage("SetTargetPos", goalPathfindingPos);
        }


        if (col.name == invName && myState != invState.INVENTORY)
        {
            myState = invState.INVENTORY;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.name == Inventory.invInstance.gameObject.name)
        {
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
        }
    }
    void OtherDragging(bool dragging)
    {
        otherDragging = dragging;
    }

    void HasWorm(bool value){
        hasWorm = value;
    }

    void LindaDistracted(bool value)
    {
        lindaDistracted = value;
    }

    void OtherCombObject(GameObject otherObject)
    {
        otherComboObject = otherObject;
    }
    void SetPosition(Vector2 pos)
    {
        myPos = pos;
    }

    void CanCombine(bool value)
    {
        canCombine = value;
    }

    void OnMouseEnter()
    {
        this.gameObject.GetComponent<Renderer>().material.color = mouseOverColor;
    }

    void OnMouseExit()
    {
        this.gameObject.GetComponent<Renderer>().material.color = originalColor;
    }

    void OnMouseDown()
    {
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        myDragging = true;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("HoldingItem", true);
    }

    void OnMouseUp()
    {
        myDragging = false;

        if (myState == invState.SLEEPING)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= 1)
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
            }
            else
                player.SendMessage("SetTargetPos", pathfindingPos);
        }
        else if(myState == invState.COMBINATION)
            StartCoroutine(wait());
        else if (myState == invState.INVENTORY)
        {
            this.gameObject.transform.position = myPos;
            Inventory.invInstance.SendMessage("SetPositions");
        }
        else if (myState == invState.DRAGGING)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;

        player.SendMessage("HoldingItem", false);
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}