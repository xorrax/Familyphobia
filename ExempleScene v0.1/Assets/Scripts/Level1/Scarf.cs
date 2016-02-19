using UnityEngine;
using System.Collections;

public class Scarf : MonoBehaviour
{
    public GameObject lockedObject;
    public GameObject goalObject;
    public Vector3 pathfindingPos;
    public string goalName;
    public string lockedName;
    public float goalDistance;

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
    private bool mouseOutside = false;
    private float treeDistance = 0f;
    private Vector2 myPos = new Vector2();
    private GameObject player;
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
        if (goalName != string.Empty)
            goalObject = GameObject.Find(goalName);

        if (lockedName != string.Empty)
            lockedObject = GameObject.Find(lockedName);

        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponent<Rigidbody2D>().mass == 1)
            mySound = lightSound;
        else if (gameObject.GetComponent<Rigidbody2D>().mass == 2)
            mySound = mediumSound;
        else if (gameObject.GetComponent<Rigidbody2D>().mass == 3)
            mySound = heavySound;
    }

    void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().WakeUp();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            if(clicked)
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
                gameObject.GetComponent<AudioSource>().Play();
                player.SendMessage("HoldingItem", false);
            }
        }

        if (onGoal && Vector3.Distance(player.transform.position, transform.position) <= treeDistance)
        {
            myState = invState.COMBINATION;
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
            this.gameObject.SetActive(false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            onGoal = false;

            if(mouseOutside)
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

    void OnTriggerStay2D(Collider2D col)
    {
        if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name && !Input.GetMouseButton(0))
        {
            if (Vector3.Distance(player.transform.position, col.transform.position) <= treeDistance)
            {
                
                myState = invState.COMBINATION;
                Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                this.gameObject.SetActive(false);
            }
            else
            {
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

    void SetTreeDistance(float dist)
    {
        treeDistance = dist;
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
        if(myState == invState.SLEEPING){
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= goalDistance)
            {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
                gameObject.GetComponent<AudioSource>().Play();
                player.SendMessage("HoldingItem", false);
            }
            else
            {
                clicked = true;
                player.SendMessage("SetTargetPos", pathfindingPos);
                player.SendMessage("HoldingItem", false);
            }
        }
        else if(myState == invState.COMBINATION)
        {
            StartCoroutine(wait());
            player.SendMessage("HoldingItem", false);
        }
        else if (myState == invState.INVENTORY)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            Debug.Log(myPos.ToString());
            player.SendMessage("HoldingItem", false);
            gameObject.transform.position = myPos;
            Debug.Log(myPos.ToString());
        }
        else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
            player.SendMessage("HoldingItem", false);
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}