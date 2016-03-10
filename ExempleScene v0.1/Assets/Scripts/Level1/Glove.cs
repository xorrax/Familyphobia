using UnityEngine;
using System.Collections;

public class Glove : MonoBehaviour
{
    public float clickDistance;
    public float flowerDist;
    public Vector3 pathfindingPos;

    private Color mouseOverColor = Color.green;
    private Color originalColor;
    private bool myDragging = false;
    private bool clicked = false;
    private bool mouseOutside = true;
    private bool scaled = false;
    private bool clickMove = false;
    private bool dragging = false;
    private bool onGoal = false;
    private float clickTimer = 0f;
    private Vector2 myPos = new Vector2();
    private Vector3 flowerPos = new Vector3();
    private GameObject player;
    private GameObject flowerCol;
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
        if (Vector3.Distance(player.transform.position, transform.position) <= clickDistance && clicked)
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

        if(Vector3.Distance(player.transform.position, flowerPos) <= flowerDist && onGoal)
        {
            myState = invState.COMBINATION;
            brokenValue++;
            onGoal = false;
            player.SendMessage("CanWalk", true);
            flowerCol.SendMessage("PickUp");
            
            if (brokenValue >= 3)
            {
                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                gameObject.SetActive(false);
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
        //if (clickMove)
        //{
        //    clickTimer += Time.deltaTime;
        //    if (clickTimer >= 0.3f)
        //    {
        //        clickMove = false;
        //        clickTimer = 0;
        //    }
        //}

        if (myDragging && myState != invState.SLEEPING)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.name == "BlueFlower" || col.name == "RedFlower" || col.name == "YellowFlower")
        {
            Debug.Log("col with flower: " + dragging.ToString());
            if(dragging)
            {
                if(!Input.GetMouseButtonUp(0))
                {
                    if(col.gameObject.activeSelf)
                    {
                        if (Vector3.Distance(player.transform.position, col.transform.position) <= flowerDist)
                        {
                            myState = invState.COMBINATION;
                            brokenValue++;
                            onGoal = false;
                            player.SendMessage("CanWalk", true);
                            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            col.SendMessage("PickUp");
                            if (brokenValue >= 3)
                            {
                                gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            Debug.Log("123");
                            flowerCol = col.gameObject;
                            col.SendMessage("GetPos");
                            player.SendMessage("SetTargetPos", flowerPos);
                            onGoal = true;
                        }
                        myState = invState.INVENTORY;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    if (col.gameObject.activeSelf)
                    {
                        if (Vector3.Distance(player.transform.position, col.transform.position) <= flowerDist)
                        {
                            Debug.Log("close click");
                            myState = invState.COMBINATION;
                            brokenValue++;
                            onGoal = false;
                            player.SendMessage("CanWalk", true);
                            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            col.SendMessage("PickUp");
                            if (brokenValue >= 3)
                            {
                                gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            Debug.Log("asd");
                            flowerCol = col.gameObject;
                            col.SendMessage("GetPos");
                            player.SendMessage("SetTargetPos", flowerPos);
                            onGoal = true;
                        }
                        myState = invState.INVENTORY;
                    }
                }
            }
        }
    }
    
    void SetFlowerPos(Vector3 pos)
    {
        flowerPos = pos;
    }

    void OnTriggerExit(Collider col)
    {

        if (col.name == "Inventory")
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
        }
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
        if(!myDragging)
        {
            myDragging = true;
            clickMove = true;
            dragging = true;
            Inventory.invInstance.holdingItem = true;
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("CanWalk", false);
    }

    void OnMouseUp()
    {
        Debug.Log("glove up");
        if(clickMove && myState != invState.SLEEPING)
        {
            Debug.Log("1");
            clickMove = false;
            myDragging = true;
            clickTimer = 0f;
            dragging = false;
            Inventory.invInstance.holdingItem = true;
        }
        else
        {
            Debug.Log("2");
            if (myDragging)
            {
                Inventory.invInstance.holdingItem = false;
            }

            myDragging = false;

            if (myState == invState.SLEEPING)
            {
                if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= clickDistance)
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
            else if (myState == invState.INVENTORY)
            {

                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                this.gameObject.transform.position = myPos;
            }
            else if (myState != invState.INVENTORY)
            {
                Inventory.invInstance.SendMessage("RemoveItem", gameObject);
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions");
                myState = invState.INVENTORY;
            }
            player.SendMessage("CanWalk", true);
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
            clicked = true;
        }
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
