using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Object : MonoBehaviour
{

    public Sprite resultSprite;
    //public GameObject combinationObject;
    //public GameObject goalObject;
    public Vector3 pathfindingPos;
    public bool mainObject;
    public string comboName;
    public string goalName;
    public float clickDistance;
    public bool locked;
    public bool warp;
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
    private bool clickMove = false;
    private bool dragging = false;
    private float clickTimer = 0f;
<<<<<<< HEAD
    private float goalDistance = 0f;
=======
    public float goalDistance;
>>>>>>> origin/master
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
        if(comboName == string.Empty)
            hasCombined = true;

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponent<Rigidbody>().mass == 1)
            gameObject.GetComponent<AudioSource>().clip = lightSound;
        else if (gameObject.GetComponent<Rigidbody>().mass == 2)
            gameObject.GetComponent<AudioSource>().clip = mediumSound;
        else if (gameObject.GetComponent<Rigidbody>().mass == 3)
            gameObject.GetComponent<AudioSource>().clip = heavySound;
    }

    void Update()
    {
<<<<<<< HEAD
        //if (combinationObject != null)
        //{
        //    if (combinationObject.activeSelf)
        //    {
        //        combinationObject.SendMessage("OtherState", (int)myState);
        //        combinationObject.SendMessage("OtherDragging", myDragging);
        //    }
        //}

=======
>>>>>>> origin/master
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

        if (onGoal && Vector3.Distance(player.transform.position, GameObject.Find(goalName).transform.position) <= goalDistance)
        {
            onGoal = false;
            if (comboName == string.Empty)
            {
                if (!Input.GetMouseButton(0))
                {
                    myState = invState.COMBINATION;
                    Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                    Inventory.invInstance.SendMessage("SetPositions");
                    this.gameObject.SetActive(false);
                    GameObject.Find(goalName).SendMessage("Goal");
                }
            }
            else if (hasCombined)
            {
                if (!Input.GetMouseButton(0))
                {
                    myState = invState.COMBINATION;
                    Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                    Inventory.invInstance.SendMessage("SetPositions");
                    this.gameObject.SetActive(false);
                    GameObject.Find(goalName).SendMessage("Goal");
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
        if (clickMove)
        {
            clickTimer += Time.deltaTime;
            if(clickTimer >= 0.5f)
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
            if(!Input.GetMouseButtonUp(0))
            {
                if (col.gameObject.activeSelf && col.gameObject.name == comboName && !locked
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
                            Inventory.invInstance.SendMessage("SetPositions");
                            myState = invState.INVENTORY;
                            this.gameObject.SetActive(false);
                        }
                        myState = invState.INVENTORY;

                    }
                }
            }
            else if (col.gameObject.activeSelf && col.gameObject.name == goalName && !locked)
            {
                if (Vector3.Distance(transform.position, col.transform.position) <= goalDistance)
                {
                    if (comboName == string.Empty)
                    {
                        if (!Input.GetMouseButtonUp(0))
                        {
                            myState = invState.COMBINATION;
                            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            player.SendMessage("CanWalk", true);
                            this.gameObject.SetActive(false);
                            onGoal = false;
                        }
                    }
                    else if (hasCombined)
                    {
                        if (!Input.GetMouseButtonUp(0))
                        {
                            myState = invState.COMBINATION;
                            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            player.SendMessage("CanWalk", true);
                            this.gameObject.SetActive(false);
                            onGoal = false;
                        }
                    }
                }
                else
                {
                    onGoal = true;
                    player.SendMessage("SetTargetPos", goalPathfindingPos);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (col.gameObject.activeSelf && col.gameObject.name == comboName && !locked
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
                            Inventory.invInstance.SendMessage("SetPositions");
                            myState = invState.INVENTORY;
                            this.gameObject.SetActive(false);
                        }
                        myState = invState.INVENTORY;

                    }
                }
                else if (col.gameObject.activeSelf && col.gameObject.name == goalName && !locked)
                {
                    if (Vector3.Distance(transform.position, col.transform.position) <= goalDistance)
                    {
                        if (comboName == string.Empty)
                        {
                            myState = invState.COMBINATION;
                            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            player.SendMessage("CanWalk", true);
                            this.gameObject.SetActive(false);
                            onGoal = false;
                        }
                        else if (hasCombined)
                        {
                            myState = invState.COMBINATION;
                            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                            player.SendMessage("CanWalk", true);
                            this.gameObject.SetActive(false);
                            onGoal = false;
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
        if (!myDragging && !Inventory.invInstance.holdingItem && myState != invState.SLEEPING)
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
<<<<<<< HEAD
        if(clickMove &&  myState != invState.SLEEPING)
=======
        if (clickMove &&  myState != invState.SLEEPING)
>>>>>>> origin/master
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
                Inventory.invInstance.SendMessage("SetPositions");
                myState = invState.INVENTORY;
            }
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;

            player.SendMessage("CanWalk", true);
            clicked = true;
        }
    }
    IEnumerator wait()
    {
        yield return 0;
    }
}