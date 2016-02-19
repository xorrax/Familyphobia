using UnityEngine;
using System.Collections;

public class FishingRod : MonoBehaviour
{

    public Sprite resultSprite;
    public GameObject combinationObject;
    public GameObject goalObject;

    public bool mainObject;
    public string comboName;
    public string goalName;

    public Vector3 pathfindingPos;

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
    private Vector2 myPos = new Vector2();
    private Vector3 goalPathfindingPos = Vector3.zero;
    private GameObject player;
    private GameObject otherComboObject;
    private string invName;
    private Sprite rodSprite;

    //FISHING ROD ONLY
    private bool hasWorm = false;
    private bool lindaDistracted = false;

    private enum invState
    {
        DRAGGING,  //..
        INVENTORY, //I inventory
        SLEEPING, //Innan man gjort något med objektet
        COMBINATION, //när man kombinerar två objekt
    };

    private invState myState = invState.SLEEPING;
    private invState otherState = invState.SLEEPING;

    private float myDistance;

    void Start()
    {
        rodSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        if (comboName != string.Empty)
            combinationObject = GameObject.Find(comboName);

        if (goalName != string.Empty)
            goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        if (gameObject.tag != "CombinationTrigger")
            canCombine = true;

        if (combinationObject == null)
            hasCombined = true;

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponent<Rigidbody2D>().mass == 1)
            mySound = lightSound;
        else if (gameObject.GetComponent<Rigidbody2D>().mass == 2)
            mySound = mediumSound;
        else if (gameObject.GetComponent<Rigidbody2D>().mass == 3)
            mySound = heavySound;

        gameObject.GetComponent<AudioSource>().clip = mySound;
    }

    void Update()
    {

        this.gameObject.GetComponent<Rigidbody2D>().WakeUp();

        if (combinationObject != null)
        {
            if (combinationObject.activeSelf)
            {
                combinationObject.SendMessage("OtherState", (int)myState);
                combinationObject.SendMessage("OtherDragging", myDragging);
            }
        }
    }

    void FixedUpdate()
    {
        if (myDragging && myState != invState.SLEEPING)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (combinationObject != null && col.gameObject.name == combinationObject.name
            && myState != invState.SLEEPING && otherState != invState.SLEEPING)
        {
            myState = invState.COMBINATION;
            if (!myDragging && !otherDragging)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = resultSprite;
                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
                hasCombined = true;
                hasWorm = true;
            }
        }
        else if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name)
        {
            if (Vector3.Distance(transform.position, col.transform.position) < 3.5f)
            {
                col.SendMessage("HasWorm", hasWorm);
                if (hasWorm && !Input.GetMouseButton(0))
                {
                    myState = invState.COMBINATION;
                    Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                    Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                    hasWorm = false;
                    gameObject.GetComponent<SpriteRenderer>().sprite = rodSprite;
                }
                else if (!hasWorm && lindaDistracted && !Input.GetMouseButton(0))
                {
                    myState = invState.COMBINATION;
                    Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                    Inventory.invInstance.SendMessage("SetPositions");
                    gameObject.SetActive(false);
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
        else if(col.name == goalObject.name)
        {
            myState = invState.DRAGGING;
        }
    }

    void OtherDragging(bool dragging)
    {
        otherDragging = dragging;
    }

    void OtherState(invState value)
    {
        otherState = value;
    }

    void OtherCombObject(GameObject otherObject)
    {
        otherComboObject = otherObject;
    }
    void SetPosition(Vector2 pos)
    {
        myPos = pos;
    }

    void SetGoalPathfindingPos(Vector3 pos)
    {
        goalPathfindingPos = pos;
    }

    void HasWorm(bool value){
        hasWorm = value;
    }

    void LindaDistracted(bool value){
        lindaDistracted = value;
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
    }

    void OnMouseUp()
    {
        myDragging = false;
        if (myState == invState.SLEEPING)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2f)
            {
                gameObject.GetComponent<AudioSource>().Play();
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
            }
            else
                player.SendMessage("SetTargetPos", pathfindingPos);
        }
        else if(myState == invState.COMBINATION)
        {
            StartCoroutine(wait());
        }
        else if (myState == invState.INVENTORY)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            this.gameObject.transform.position = myPos;
        }
        else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
        //Inventory.invInstance.SendMessage("EnableCol");
    }
    IEnumerator wait()
    {
        yield return 0;
    }
}