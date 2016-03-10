using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
    public GameObject combinationObject;
    public GameObject goalObject;
    public GameObject linda;

    public Vector3 pathfindingPos;
    public Vector3 goalPathfindingPos;
    public float goalDistance;
    public bool mainObject;
    public string comboName;
    public string goalName;

    private Color mouseOverColor = Color.green;
    private Color originalColor;

    private bool myDragging = false;
    private bool canCombine = false;
    private bool hasCombined = false;
    private bool hasWorm = false;
    private Vector2 myPos = new Vector2();
    private GameObject player;
    private GameObject otherComboObject;
    private string invName;
    private bool pickedUp = false;
    private bool onGoal = false;
    private bool onCombine = false;
    private bool scaled = false;
    private bool lindaDistracted = false;
    private bool lastLindaDistraced = true;
    private bool clickMove = false;
    private bool dragging = false;
    private float clickTimer = 0f;

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
        if (comboName != string.Empty && combinationObject == null)
            combinationObject = GameObject.Find(comboName);

        if (goalName != string.Empty && goalObject == null)
            goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        if (gameObject.tag != "CombinationTrigger")
            canCombine = true;

        player = GameObject.FindGameObjectWithTag("Player");
        if(linda == null)
            linda = GameObject.Find("Entrance_Linda");
    }

    void Update()
    {
        if(Vector3.Distance(player.transform.position, goalObject.transform.position) <= goalDistance && onGoal)
        {
            Debug.Log("key on goal");
            onGoal = false;
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
            this.gameObject.SetActive(false);
        }

        if (Vector3.Distance(player.transform.position, pathfindingPos) <= 0.5f && onCombine)
        {
            if (hasWorm)
            {
                linda.SendMessage("HasWorm");
            }
            else if (!hasWorm && lindaDistracted)
            {
                if (scaled)
                {
                    scaled = false;
                    gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                }
                player.SendMessage("FishAnimation", gameObject);
                player.SendMessage("CanWalk", true);
                Inventory.invInstance.SendMessage("AddItem", gameObject);
                myState = invState.INVENTORY;
                onCombine = false;
                pickedUp = true;
                Inventory.invInstance.SendMessage("RemoveItem", combinationObject);
                Inventory.invInstance.SendMessage("SetPositions");
                combinationObject.SetActive(false);
                
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            onCombine = false;
            onGoal = false;
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

        if (myDragging && myState != invState.SLEEPING && pickedUp)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnTriggerStay(Collider col)
    {
<<<<<<< HEAD
        if (dragging)
        {
=======
        if(col.name == comboName)
        {
            GameObject.Find(comboName).SendMessage("LindaDistracted", lindaDistracted);
        }

        if (dragging)
        {
>>>>>>> origin/master
            if (combinationObject != null && col.gameObject.name == combinationObject.name && Input.GetMouseButtonUp(0))
            {
                if (Vector3.Distance(player.transform.position, pathfindingPos) <= 0.5f)
                {
                    if (!hasWorm && lindaDistracted)
                    {
                        if (scaled)
                        {
                            scaled = false;
                            gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                        }
                        player.SendMessage("FishAnimation");
                        player.SendMessage("CanWalk", true);
                        Inventory.invInstance.SendMessage("AddItem", gameObject);
                        myState = invState.INVENTORY;
                        Inventory.invInstance.SendMessage("RemoveItem", combinationObject);
                        Inventory.invInstance.SendMessage("SetPositions");
                        pickedUp = true;
                        combinationObject.SetActive(false);
                    }
                }
                else
                {
                    player.SendMessage("SetTargetPos", pathfindingPos);
                    onCombine = true;
                }
            }
            else if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name)
            {
                myState = invState.COMBINATION;
                if (Input.GetMouseButtonUp(0))
                {
                    if (Vector3.Distance(player.transform.position, col.transform.position) < goalDistance)
                    {
                        if (combinationObject == null && otherComboObject == null)
                        {

                            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                            Debug.Log("key on goal");
                            this.gameObject.SetActive(false);
                            //DO stuff!!
                            //!     Skriv kod här.. lås upp dörr
                            //! 
                        }
                    }
                    else
                    {
                        onGoal = true;
                        player.SendMessage("SetTargetPos", goalPathfindingPos);
                    }
                }
            }
        }
        else
        {   if(Input.GetMouseButton(0))
            {
                if (col.gameObject.name == comboName)
                {
                    if (Vector3.Distance(player.transform.position, pathfindingPos) <= 0.5f)
<<<<<<< HEAD
                    {
                        if (!hasWorm && lindaDistracted)
                        {
                            if (scaled)
                            {
                                scaled = false;
                                gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                            }
                            player.SendMessage("FishAnimation");
                            player.SendMessage("CanWalk", true);
                            Inventory.invInstance.SendMessage("AddItem", gameObject);
                            myState = invState.INVENTORY;
                            Inventory.invInstance.SendMessage("RemoveItem", combinationObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            pickedUp = true;
                            combinationObject.SetActive(false);
                        }
                    }
                    else
                    {
=======
                    {
                        if (!hasWorm && lindaDistracted)
                        {
                            if (scaled)
                            {
                                scaled = false;
                                gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                            }
                            player.SendMessage("FishAnimation");
                            player.SendMessage("CanWalk", true);
                            Inventory.invInstance.SendMessage("AddItem", gameObject);
                            myState = invState.INVENTORY;
                            Inventory.invInstance.SendMessage("RemoveItem", combinationObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            pickedUp = true;
                            combinationObject.SetActive(false);
                        }
                    }
                    else
                    {
>>>>>>> origin/master
                        player.SendMessage("SetTargetPos", pathfindingPos);
                        onCombine = true;
                    }
                }
                else if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name)
                {
                    myState = invState.COMBINATION;
                    if (Vector3.Distance(player.transform.position, col.transform.position) < goalDistance)
                    {
                        if (combinationObject == null && otherComboObject == null)
                        {

                            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                            this.gameObject.SetActive(false);
                            //DO stuff!!
                            //!     Skriv kod här.. gå till bi pussel
                            //! 
                        }
                    }
                    else
                    {
                        onGoal = true;
                        player.SendMessage("SetTargetPos", goalPathfindingPos);
                    }
                    
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

    void HasWorm(bool value){
        hasWorm = value;
    }

    void LindaDistracted(bool value)
    {
        lindaDistracted = value;
        GameObject temp = GameObject.Find(comboName);
        if(temp != null)
            temp.SendMessage("LindaDistracted", lindaDistracted);
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
        if(myState == invState.SLEEPING)
        {
            scaled = true;
            gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }

    }

    void OnMouseExit()
    {
        if(myState == invState.SLEEPING && scaled)
        {
            scaled = false;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1);
        }
    }

    void OnMouseDown()
    {
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        if(!myDragging && myState != invState.SLEEPING)
        {
            clickMove = true;
            myDragging = true;
            dragging = true;
            Inventory.invInstance.holdingItem = true;
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
            clickTimer = 0f;
            Inventory.invInstance.holdingItem = true;
        }
        else
        {
            if (myDragging)
                Inventory.invInstance.holdingItem = false;

            myDragging = false;

            if (myState == invState.COMBINATION)
            {
                StartCoroutine(wait());
            }
            else if (myState == invState.INVENTORY)
            {
                this.gameObject.transform.position = myPos;
                Inventory.invInstance.AddItem(gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
            }
            else if (myState == invState.DRAGGING)
            {
                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                myState = invState.INVENTORY;
            }
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;

            player.SendMessage("CanWalk", true);
        }
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}