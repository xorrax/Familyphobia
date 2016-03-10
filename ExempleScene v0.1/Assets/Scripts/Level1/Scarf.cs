using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scarf : MonoBehaviour
{
    public Vector3 pathfindingPos;
    public Vector3 goalPathfindingPos;
    public string goalName;
    public string lockedName;
    public float clickDistance;
    public float goalDistance;
    public Sprite treeSprite;

    //Sound
    public AudioClip lightSound;
    public AudioClip mediumSound;
    public AudioClip heavySound;

    private AudioClip mySound;
    //------

    private Color mouseOverColor = Color.green;
    private Color originalColor;
    private bool myDragging = false;
    private bool clicked = false;
    private bool onGoal = false;
    private bool scaled = false;
    private bool mouseOutside = false;
    private bool clickMove = false;
    private bool dragging = false;
    private bool clickedGoal = false;
    private float clickTimer = 0f;
    private Vector2 myPos = new Vector2();
    private GameObject player;
    private Vector3 colPos = new Vector3();
    private GameObject colObject;
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
        //if (goalName != string.Empty)
        //    goalObject = GameObject.Find(goalName);

        //if (lockedName != string.Empty)
        //    lockedObject = GameObject.Find(lockedName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponent<Rigidbody>().mass == 1)
            mySound = lightSound;
        else if (gameObject.GetComponent<Rigidbody>().mass == 2)
            mySound = mediumSound;
        else if (gameObject.GetComponent<Rigidbody>().mass == 3)
            mySound = heavySound;
    }

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Dreamworld_Level01")
        {
            //goalObject = GameObject.Find(goalName);
            //lockedObject = GameObject.Find(lockedName);
            //Debug.Log(lockedObject.ToString() + " : " + goalObject.ToString());
            //Debug.Log("warp scarf");
        }
    }

    void Update()
    {
        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= clickDistance)
        {
            if(clicked)
            {
                if (scaled)
                {
                    scaled = false;
                    gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                }
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
                gameObject.GetComponent<AudioSource>().Play();
                player.SendMessage("CanWalk", true);
                clicked = false;
            }
        }

        if (onGoal && Vector3.Distance(player.transform.position, colObject.transform.position) <= goalDistance)
        {
            myState = invState.COMBINATION;
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
            onGoal = false;
            GameObject.Find(lockedName).SendMessage("Locked", false);
            player.SendMessage("CanWalk", true);
            this.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (clickedGoal)
            {
                onGoal = false;
                clickedGoal = false;
            }
            if (mouseOutside)
                clicked = false;
        }
    }

    void FixedUpdate()
    {
        if (clickMove)
        {
            clickTimer += Time.deltaTime;
            if (clickTimer >= 0.5f)
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
        if (dragging)
        {
            if (col.gameObject.activeSelf && col.gameObject.name == goalName && !Input.GetMouseButton(0))
            {
                myState = invState.COMBINATION;
                if (Vector3.Distance(player.transform.position, col.transform.position) <= goalDistance)
                {
                    col.gameObject.GetComponent<SpriteRenderer>().sprite = treeSprite;
                    GameObject.Find(lockedName).SendMessage("Locked", false);
                    player.SendMessage("CanWalk", true);
                    this.gameObject.SetActive(false);
                }
                else
                {
                    onGoal = true;
                    player.SendMessage("SetTargetPos", goalPathfindingPos);
                    Inventory.invInstance.AddItem(gameObject);
                    Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                    Inventory.invInstance.holdingItem = false;
                    myDragging = false;
                    myState = invState.INVENTORY;
                    colObject = col.gameObject;
                }
            }
        }
        else
        {
            if (col.gameObject.name == goalName && Input.GetMouseButton(0))
            {
                myState = invState.COMBINATION;
                if (Vector3.Distance(player.transform.position, col.transform.position) <= goalDistance)
                {
                    col.gameObject.GetComponent<SpriteRenderer>().sprite = treeSprite;
                    GameObject.Find(lockedName).SendMessage("Locked", false);
                    player.SendMessage("CanWalk", true);
                    this.gameObject.SetActive(false);
                }
                else
                {
                    onGoal = true;
                    player.SendMessage("SetTargetPos", goalPathfindingPos);
                    Inventory.invInstance.AddItem(gameObject);
                    Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                    Inventory.invInstance.holdingItem = false;
                    myDragging = false;
                    myState = invState.INVENTORY;
                    colObject = col.gameObject;
                }
            }
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
        if (!myDragging && !Inventory.invInstance.holdingItem && myState != invState.SLEEPING)
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
                Inventory.invInstance.SendMessage("AddItem", gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                gameObject.transform.position = myPos;
            }
            else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
            {
                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                myState = invState.INVENTORY;
            }
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
            clickedGoal = true;
        }
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}