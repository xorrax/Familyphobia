using UnityEngine;
using System.Collections;

public class Glove : MonoBehaviour
{
    public float goalDistance;
    public Vector3 pathfindingPos;

    private Color mouseOverColor = Color.green;
    private Color originalColor;
    private bool myDragging = false;
    private bool clicked = false;
    private bool mouseOutside = true;
    private bool scaled = false;
    private Vector2 myPos = new Vector2();
    private GameObject player;
    private int brokenValue = 0;
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
        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance && clicked)
        {
            if (scaled)
            {
                scaled = false;
                gameObject.transform.localScale = new Vector3(1f, 1f, 1);
            }
            clicked = false;
            Inventory.invInstance.SendMessage("AddItem", gameObject);
            myState = invState.INVENTORY;
            gameObject.GetComponent<AudioSource>().Play();
        }

        if (Input.GetMouseButtonDown(0) && mouseOutside)
            clicked = false;
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
    
    void OnColliderStay(Collider col)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (col.tag == "flower")
                myState = invState.COMBINATION;
        }
    }

    void OnFlower(GameObject col)
    {
        myState = invState.COMBINATION;
        brokenValue++;
        if (brokenValue >= 3)
        {
            gameObject.SetActive(false);
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
        }

        Debug.Log("on flower");
    }


    void OnTriggerExit(Collider col)
    {

        if (col.name == "Inventory")
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
            Debug.Log("glove exit inv");
        }
    }

    void SetPosition(Vector2 pos)
    {
        myPos = pos;
    }

    void OnMouseEnter()
    {
        //this.gameObject.GetComponent<Renderer>().material.color = mouseOverColor;
        mouseOutside = false;

        if (myState == invState.SLEEPING)
        {
            scaled = true;
            gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }
    }

    void OnMouseExit()
    {
        this.gameObject.GetComponent<Renderer>().material.color = originalColor;
        mouseOutside = true;

        if (myState == invState.SLEEPING)
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

        if (myState == invState.SLEEPING)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= goalDistance)
            {
                if (scaled)
                {
                    scaled = false;
                    gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                }
                Inventory.invInstance.SendMessage("AddItem", gameObject);
                myState = invState.INVENTORY;
                gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                clicked = true;
                player.SendMessage("SetTargetPos", pathfindingPos);
            }
        }
        else if(myState == invState.COMBINATION)
        {
            StartCoroutine(wait());
        }
        else if (myState == invState.INVENTORY)
        {
            this.gameObject.transform.position = myPos;
            Inventory.invInstance.SendMessage("SetPositions");
        }
        else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
        {
            StartCoroutine(wait());
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", gameObject);
            myState = invState.INVENTORY;
        }
        player.SendMessage("CanWalk", true);
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
    }

    IEnumerator wait()
    {
        yield return 0;
    }

    IEnumerator endOfFrame()
    {
        yield return new WaitForEndOfFrame();
    }
}
