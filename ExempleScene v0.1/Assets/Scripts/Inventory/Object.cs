using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Object : MonoBehaviour
{

    public Sprite resultSprite;
    public GameObject combinationObject;
    public GameObject goalObject;
    public Vector3 pathfindingPos;
    public bool mainObject;
    public string comboName;
    public string goalName;
    public float clickDistance;
    public bool locked;
    //Sound
    public AudioClip lightSound;
    public AudioClip mediumSound;
    public AudioClip heavySound;
    //------

    private Color mouseOverColor = Color.green;
    private Color originalColor;
    private bool myDragging = false;
    private bool otherDragging = false;
    private bool hasCombined = false;
    private Vector3 goalPathfindingPos = Vector3.zero;
    private Vector2 myPos = new Vector2();
    private GameObject player;
    private bool clicked = false;
    private bool mouseOutside = true;
    private bool onGoal = false;
    private bool scaled = false;
    private float goalDistance = 0f;
    private enum invState
    {
        DRAGGING,  //..
        INVENTORY, //I inventory
        SLEEPING, //Innan man gjort något med objektet
        COMBINATION, //när man kombinerar två objekt
    };
    private invState otherState = invState.SLEEPING;
    private invState myState = invState.SLEEPING;

    private float myDistance;

    void Start()
    {
        if (comboName != string.Empty)
            combinationObject = GameObject.Find(comboName);

        if (goalName != string.Empty)
            goalObject = GameObject.Find(goalName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        if (combinationObject == null)
            hasCombined = true;

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponent<Rigidbody>().mass == 1)
            gameObject.GetComponent<AudioSource>().clip = lightSound;
        else if (gameObject.GetComponent<Rigidbody>().mass == 2)
            gameObject.GetComponent<AudioSource>().clip = mediumSound;
        else if (gameObject.GetComponent<Rigidbody>().mass == 3)
            gameObject.GetComponent<AudioSource>().clip = heavySound;

        //if (goalObject != null)
        //    goalObject.SendMessage("SetGoalPathfindingPos", pathfindingPos);
    }

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "Dreamworld_Level01" && tag == "ItemWarp")
        {
            if (goalName != string.Empty)
                goalObject = GameObject.Find(goalName);
            if (comboName != string.Empty)
                combinationObject = GameObject.Find(comboName);
        }
    }

    void Update()
    {
        if (combinationObject != null)
        {
            if (combinationObject.activeSelf)
            {
                combinationObject.SendMessage("OtherState", (int)myState);
                combinationObject.SendMessage("OtherDragging", myDragging);
            }
        }

        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= clickDistance && !locked)
        {
            if (scaled)
            {
                scaled = false;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1);
            }

            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
            clicked = false;
            
        }

        if (onGoal && Vector3.Distance(player.transform.position, goalObject.transform.position) <= goalDistance)
        {
            onGoal = false;
            if (combinationObject == null)
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

            if (mouseOutside)
                clicked = false;
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

    void OnTriggerStay(Collider col)
    {

        if (combinationObject != null && combinationObject.gameObject.activeSelf && col.gameObject.name == combinationObject.name && !locked
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
        else if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name && !locked)
        {
            if (Vector3.Distance(transform.position, col.transform.position) <= goalDistance)
            {
                if (combinationObject == null)
                {
                    if (!Input.GetMouseButton(0))
                    {
                        myState = invState.COMBINATION;
                        Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                        Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                        player.SendMessage("CanWalk", true);
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
                        player.SendMessage("CanWalk", true);
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


        if (col.name == "Inventory" && myState != invState.INVENTORY)
        {
            myState = invState.INVENTORY;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Inventory")
        {
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
            Debug.Log(gameObject.name + " exit inv");
        }
    }


    void SetGoalPathfindingPos(Vector3 pos)
    {
        goalPathfindingPos = pos;
    }

    void SetGoalDistance(float distance)
    {
        goalDistance = distance;
    }

    void OtherState(invState value)
    {
        otherState = value;
    }
    void OtherDragging(bool dragging)
    {
        otherDragging = dragging;
    }

    void SetPosition(Vector2 pos)
    {
        myPos = pos;
    }

    void Locked(bool value)
    {
        locked = value;
    }

    void OnMouseEnter()
    {
        mouseOutside = false;

        if (myState == invState.SLEEPING)
        {
            scaled = true;
            gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }
    }

    void OnMouseExit()
    {
        mouseOutside = true;
        if (myState == invState.SLEEPING && scaled)
        {
            scaled = false;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1);
        }
    }

    void OnMouseDown()
    {
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        myDragging = true;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("CanWalk", false);
    }

    void OnMouseUp()
    {
        myDragging = false;

        if (myState == invState.SLEEPING && !locked)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= clickDistance)
            {
                if (scaled)
                {
                    scaled = false;
                    gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                }

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
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            this.gameObject.transform.position = myPos;
        }
        else if (myState == invState.DRAGGING)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;

        player.SendMessage("CanWalk", true);
        clicked = true;
    }

    void Warp(string level)
    {
        if (level == "Dreamworld_Level01")
        {
            if (goalObject == null)
                goalObject = GameObject.Find(goalName);

            if (combinationObject == null)
                combinationObject = GameObject.Find(comboName);
        }
    }
    IEnumerator wait()
    {
        yield return 0;
    }
}