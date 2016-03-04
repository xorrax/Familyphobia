using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FishingRod : MonoBehaviour
{

    public Sprite invSprite;
    public Sprite wormSprite;
    public GameObject combinationObject;
    public GameObject goalObject;

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

        if (comboName != string.Empty && combinationObject == null)
            combinationObject = GameObject.Find(comboName);

        if (goalName != string.Empty && goalObject == null)
            goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "Dreamworld_Level01")
        {
            goalObject = GameObject.Find(goalName);
        }
    }

    void Update()
    {
        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            if (scaled)
            {
                scaled = false;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1);
            }
            transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0), Vector3.forward);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = invSprite;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.GetComponent<BoxCollider>().size = new Vector3(0.55f, 0.86f, 0.2f);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
        }

        if (Vector3.Distance(player.transform.position, goalPathfindingPos) <= 0.5f && onGoal)
        {
            if (hasWorm && !Input.GetMouseButton(0))
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                hasWorm = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = rodSprite;

                Debug.Log("1");
            }
            else if (!hasWorm && !lindaDistracted && !Input.GetMouseButton(0))
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                Debug.Log("3");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            clicked = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Debug.Log("fishing rod: " + hasWorm.ToString() + " : " + lindaDistracted.ToString());
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
        if (combinationObject != null && col.gameObject.name == combinationObject.name
            && myState != invState.SLEEPING && otherState != invState.SLEEPING)
        {
            myState = invState.COMBINATION;
            if (!myDragging)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = wormSprite;
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
                hasWorm = true;
                goalObject.SendMessage("HasWorm", hasWorm);
            }

        }
        else if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name)
        {
            myState = invState.COMBINATION;
            goalObject.SendMessage("HasWorm", hasWorm);
            if (hasWorm && !Input.GetMouseButton(0))
            {
                if (Vector3.Distance(player.transform.position, goalPathfindingPos) <= 0.5f)
                {
                    Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                    hasWorm = false;
                    gameObject.GetComponent<SpriteRenderer>().sprite = rodSprite;
                }
                else
                    onGoal = true;
                Debug.Log("1");
            }
            else if (!hasWorm && !lindaDistracted && !Input.GetMouseButton(0))
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                Debug.Log("3");
            }

            if (Input.GetMouseButtonUp(0))
                Inventory.invInstance.SendMessage("SetPositions");
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
        else if (goalObject != null)
        {
            if (col.name == goalObject.name)
            {
                myState = invState.DRAGGING;
            }
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
    }

    void OnMouseExit()
    {
        if (myState == invState.SLEEPING && scaled)
        {
            scaled = false;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * (1 / 1.1f), gameObject.transform.localScale.y * (1 / 1.1f), 1);
        }
    }

    void OnMouseDown()
    {
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        if (!myDragging)
        {
            myDragging = true;
            clickMove = true;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("CanWalk", false);
    }

    void OnMouseUp()
    {
        if (clickMove)
        {
            myDragging = true;
            clickMove = false;
            clickTimer = 0f;
        }
        else
        {
            myDragging = false;
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
                Inventory.invInstance.SendMessage("SetPositions");
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
            player.SendMessage("CanWalk", true);
        }
    }

    void NewScene()
    {
        if (comboName != string.Empty && combinationObject == null)
            combinationObject = GameObject.Find(comboName);

        if (goalName != string.Empty && goalObject == null)
            goalObject = GameObject.Find(goalName);
    }

    void Warp(string level)
    {
        if (level == "Dreamworld_Level01")
        {
            if (goalObject == null)
                goalObject = GameObject.Find(goalName);
        }
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}