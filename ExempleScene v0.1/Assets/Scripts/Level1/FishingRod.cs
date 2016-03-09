using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FishingRod : MonoBehaviour
{

    public Sprite invSprite;
    public Sprite wormSprite;

    public bool mainObject;
    public string comboName;
    public string goalName;
    public float goalDistance;

    public Vector3 pathfindingPos;

    private Color mouseOverColor = Color.green;
    private Color originalColor;

    private bool myDragging = false;
    private bool canCombine = false;
    private bool hasCombined = false;
    private bool clicked = false;
    private bool onGoal = false;
    private bool scaled = false;
    private bool clickMove = false;
    private bool goalClicked = false;
    private bool dragging = false;
    private bool mouseOutside = true;
    private float clickTimer = 0f;
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

        //if (comboName != string.Empty && combinationObject == null)
        //    combinationObject = GameObject.Find(comboName);

        //if (goalName != string.Empty && goalObject == null)
        //    goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnLevelWasLoaded()
    {
    }

    void Update()
    {   
        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            if (scaled)
            {
                scaled = false;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(0.55f, 0.86f, 0.2f);
            }
            transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0), Vector3.forward);
            gameObject.GetComponent<SpriteRenderer>().sprite = invSprite;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            player.SendMessage("CanWalk", true);
            gameObject.GetComponent<AudioSource>().Play();
            myState = invState.INVENTORY;
            clicked = false;
        }

        if (Vector3.Distance(player.transform.position, goalPathfindingPos) <= goalDistance && onGoal)
        {
            if (hasWorm)
            {
                myState = invState.COMBINATION;
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                hasWorm = false;
                player.SendMessage("CanWalk", true);
                gameObject.GetComponent<SpriteRenderer>().sprite = rodSprite;
            }
            else if (!hasWorm)
            {
                myState = invState.COMBINATION;
                Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                player.SendMessage("CanWalk", true);
                onGoal = false;
                gameObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(goalClicked)
            {
                onGoal = false;
                goalClicked = false;
            }
            clicked = false;
        }

        if(Input.GetMouseButton(0))
        {
            if (!mouseOutside)
                dragging = true;
        }
    }

    void FixedUpdate()
    {
        if (clickMove)
        {
            clickTimer += Time.deltaTime;
            if (clickTimer >= 1)
            {
                clickMove = false;
                clickTimer = 0;
            }
        }

        if (myDragging && myState != invState.SLEEPING)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.name == goalName)
        {
            GameObject.Find(goalName).SendMessage("HasWorm", hasWorm);
        }

        if (dragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (col.gameObject.activeSelf && col.gameObject.name == comboName
                    && myState != invState.SLEEPING && otherState != invState.SLEEPING)
                {
                    myState = invState.COMBINATION;
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = wormSprite;
                    Inventory.invInstance.AddItem(gameObject);
                    Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                    Inventory.invInstance.holdingItem = false;
                    myState = invState.INVENTORY;
                    hasWorm = true;
                    GameObject.Find(goalName).SendMessage("HasWorm", hasWorm);
                }
                else if (col.gameObject.activeSelf && col.gameObject.name == goalName)
                {
                    myState = invState.COMBINATION;
                    if (hasWorm)
                    {
                        if (Vector3.Distance(player.transform.position, goalPathfindingPos) <= 0.5f)
                        {
                            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                            hasWorm = false;
                            gameObject.GetComponent<SpriteRenderer>().sprite = rodSprite;
                            GameObject.Find(goalName).SendMessage("HasWorm", hasWorm);
                        }
                        else
                        {
                            Inventory.invInstance.AddItem(gameObject);
                            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                            Inventory.invInstance.holdingItem = false;
                            onGoal = true;
                        }
                    }
                    else if (!hasWorm && !lindaDistracted)
                    {
                        Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                        Inventory.invInstance.SendMessage("SetPositions");
                    }

                    
                    Inventory.invInstance.SendMessage("SetPositions");
                }
            }
        }
        else if(Input.GetMouseButton(0))
        {
            if (col.gameObject.name == comboName && col.gameObject.activeSelf
                && myState != invState.SLEEPING && otherState != invState.SLEEPING)
            {
                myState = invState.COMBINATION;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = wormSprite;
                Inventory.invInstance.AddItem(gameObject);
                Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                Inventory.invInstance.holdingItem = false;
                myState = invState.INVENTORY;
                hasWorm = true;
            }
            else if (col.gameObject.activeSelf && col.gameObject.name == goalName)
            {
                myState = invState.COMBINATION;
                GameObject.Find(goalName).SendMessage("HasWorm", hasWorm);
                if (hasWorm)
                {
                    if (Vector3.Distance(player.transform.position, goalPathfindingPos) <= 0.5f)
                    {
                        Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                        hasWorm = false;
                        GameObject.Find(goalName).SendMessage("HasWorm", hasWorm);
                        gameObject.GetComponent<SpriteRenderer>().sprite = rodSprite;
                    }
                    else
                        onGoal = true;
                }
                else if (!hasWorm && !lindaDistracted)
                {
                    Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                    Inventory.invInstance.SendMessage("SetPositions");
                }
            }
            
        }
        if (col.name == invName && myState != invState.INVENTORY)
        {
            myState = invState.INVENTORY;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == Inventory.invInstance.gameObject.name)
        {
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
        }
    }

    void OtherState(invState value)
    {
        otherState = value;
    }
    void SetPosition(Vector2 pos)
    {
        myPos = pos;
    }

    void SetGoalPathfindingPos(Vector3 pos)
    {
        goalPathfindingPos = pos;
    }

    void HasWorm(bool value)
    {
        hasWorm = value;
    }

    void LindaDistracted(bool value)
    {
        lindaDistracted = value;
    }

    void OnMouseEnter()
    {
        if (myState == invState.SLEEPING)
        {
            scaled = true;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 1.1f, gameObject.transform.localScale.y * 1.1f, 1);
        }
        mouseOutside = false;
    }

    void OnMouseExit()
    {
        if (myState == invState.SLEEPING && scaled)
        {
            scaled = false;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * (1 / 1.1f), gameObject.transform.localScale.y * (1 / 1.1f), 1);
        }
        mouseOutside = true;
    }

    void OnMouseDown()
    {
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        if (!myDragging)
        {
            Inventory.invInstance.holdingItem = true;
            myDragging = true;
            clickMove = true;
            dragging = true;
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("CanWalk", false);
    }

    void OnMouseUp()
    {
        if (clickMove && myState != invState.SLEEPING)
        {
            myDragging = true;
            clickMove = false;
            clickTimer = 0;
            dragging = false;
            Inventory.invInstance.holdingItem = true;
        }
        else
        {
            if (myDragging)
            {
                Inventory.invInstance.holdingItem = false;
            }

            myDragging = false;
            player.SendMessage("CanWalk", true);
            if (myState == invState.SLEEPING)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
                {
                    if (scaled)
                    {
                        scaled = false;
                        gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                        transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0), Vector3.forward);
                    }

                    this.gameObject.GetComponent<SpriteRenderer>().sprite = invSprite;
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    gameObject.GetComponent<BoxCollider>().size = new Vector3(0.55f, 0.86f, 0.2f);
                    Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                    myState = invState.INVENTORY;
                    gameObject.GetComponent<AudioSource>().Play();
                }
                else
                {
                    clicked = true;
                    player.SendMessage("SetTargetPos", pathfindingPos);
                }
            }
            else if (myState == invState.COMBINATION)
            {
                StartCoroutine(wait());
            }
            else if (myState == invState.INVENTORY)
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                this.gameObject.transform.position = myPos;
            }
            else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
            {
                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                myState = invState.INVENTORY;
            }
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
        }
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}