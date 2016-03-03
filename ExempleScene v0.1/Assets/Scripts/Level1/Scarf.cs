﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scarf : MonoBehaviour
{
    public GameObject lockedObject;
    public GameObject goalObject;
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
    private bool findObjects = false;
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
            goalObject = GameObject.Find(goalName);
            lockedObject = GameObject.Find(lockedName);
            Debug.Log(lockedObject.ToString() + " : " + goalObject.ToString());
            Debug.Log("warp scarf");
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

        if (onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            myState = invState.COMBINATION;
            goalObject.GetComponent<SpriteRenderer>().sprite = treeSprite;
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
            onGoal = false;
            lockedObject.SendMessage("Locked", false);
            player.SendMessage("CanWalk", true);
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

    void OnTriggerStay(Collider col)
    {
        Debug.Log("scarf collision: " + col.name);
        if (goalObject != null && goalObject.activeSelf && col.gameObject.name == goalObject.name && !Input.GetMouseButton(0))
        {
            Debug.Log("scarf on tree");
            myState = invState.COMBINATION;
            if (Vector3.Distance(player.transform.position, col.transform.position) <= goalDistance)
            {
                
                goalObject.GetComponent<SpriteRenderer>().sprite = treeSprite;
                Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
                Inventory.invInstance.SendMessage("SetPositions", this.gameObject);
                lockedObject.SendMessage("Locked", false);
                player.SendMessage("CanWalk", true);
                this.gameObject.SetActive(false);
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
        if (myState == invState.SLEEPING)
        {
            scaled = true;
            gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }
    }

    void OnMouseExit()
    {
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
        player.SendMessage("CanWalk", true);
        if (myState == invState.SLEEPING){
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
        else if(myState == invState.COMBINATION)
        {
            StartCoroutine(wait());
        }
        else if (myState == invState.INVENTORY)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            gameObject.transform.position = myPos;
        }
        else if (myState != invState.INVENTORY && myState != invState.COMBINATION)
        {
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
        }
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
    }

    void Warp(string level)
    {
        if (level == "Dreamworld_Level01")
        {
            findObjects = true;
        }
    }

    IEnumerator wait()
    {
        yield return 0;
    }
}