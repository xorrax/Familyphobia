using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Nail : MonoBehaviour {

    private Sprite nailSprite;
    public Sprite nailHasFrame;
    public Sprite nailHasMotherPicture;
    public Sprite nailGoodPicture;
    public Sprite nailUglyPicture;
    public float goalDistance;
    public GameObject goodPicture;
    public Vector3 pathfindingPos;

    private GameObject player;
    private bool onGoal = false;
    private bool objectDragging = false;
    private bool gotFlowers = false;
    private int tempTimer = 0;
    private string colName = string.Empty;
    enum nailStates
    {
        empty,
        hasFrame,
        hasPicture,
        hasBouquet,
        uglyPicture,
    }

    nailStates myState;

    void Start(){
        myState = nailStates.empty;
        nailSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject tempObject = GameObject.Find("EmptyFrame");
        tempObject.SendMessage("SetGoalDistance", goalDistance);
        tempObject.SendMessage("SetGoalPathfindingPos", pathfindingPos);
        tempObject = GameObject.Find("Bouquet");
        tempObject.SendMessage("SetGoalDistance", goalDistance);
        tempObject.SendMessage("SetGoalPathfindingPos", pathfindingPos);
    }

    void FixedUpdate()
    {
        switch(myState)
        {
            case nailStates.uglyPicture:
                {
                    tempTimer++;
                    if (tempTimer >= 300)
                    {
                        tempTimer = 0;
                        myState = nailStates.hasFrame;
                        gameObject.GetComponent<SpriteRenderer>().sprite = nailHasFrame;
                    }

                    break;
                }
        }
    }

    void Update()
    {
        if(onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            switch(myState)
            {
                case nailStates.empty:
                    {
                        if(colName == "EmptyFrame")
                        {
                            Debug.Log("emptyframe, update");
                            gameObject.GetComponent<SpriteRenderer>().sprite = nailHasFrame;
                            myState = nailStates.hasFrame;
                        }
                        break;
                    }
                case nailStates.hasFrame:
                    {
                        if (colName == "MotherPicture")
                        {
                            gameObject.GetComponent<SpriteRenderer>().sprite = nailHasMotherPicture;
                            myState = nailStates.hasPicture;
                        }
                        else if (colName == "Bouquet")
                        {
                            GameObject tempObject = GameObject.Find("Bouquet");
                            gameObject.GetComponent<SpriteRenderer>().sprite = nailUglyPicture;
                            myState = nailStates.uglyPicture;
                            Inventory.invInstance.SendMessage("AddItem", tempObject);
                            Inventory.invInstance.SendMessage("SetPositions");
                        }
                        break;
                    }
                case nailStates.hasPicture:
                    {
                        if (colName == "Bouquet")
                        {
                            if (gotFlowers)
                            {
                                GameObject tempObject = GameObject.Find("Bouquet");
                                gameObject.GetComponent<SpriteRenderer>().sprite = nailSprite;
                                myState = nailStates.hasBouquet;
                                goodPicture.SetActive(true);
                                goodPicture.transform.position = gameObject.transform.position;

                                if (tempObject.activeSelf)
                                    tempObject.SendMessage("RemoveBouquet");
                            }
                            else
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = nailUglyPicture;
                                myState = nailStates.uglyPicture;
                            }
                        }
                        break;
                    }
            }
        }
    }
    void SetDragging(bool value)
    {
        objectDragging = value;
    }

    void GotFlowers()
    {
        gotFlowers = true;
    }

    void OnTriggerStay(Collider col)
    {
        if (!Input.GetMouseButton(0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
            {
                switch (myState)
                {
                    case nailStates.empty:
                        {
                            if (col.name == "EmptyFrame")
                            {
                                Debug.Log("emptyframe, col");
                                gameObject.GetComponent<SpriteRenderer>().sprite = nailHasFrame;
                                myState = nailStates.hasFrame;
                            }
                            break;
                        }
                    case nailStates.hasFrame:
                        {
                            if (col.name == "MotherPicture")
                            {
                                col.gameObject.SetActive(false);
                                gameObject.GetComponent<SpriteRenderer>().sprite = nailHasMotherPicture;
                                myState = nailStates.hasPicture;
                            }
                            else if (col.name == "Bouquet")
                            {
                                gameObject.GetComponent<SpriteRenderer>().sprite = nailUglyPicture;
                                myState = nailStates.uglyPicture;
                                Inventory.invInstance.SendMessage("AddItem", col.gameObject);
                                Inventory.invInstance.SendMessage("SetPositions");
                            }
                            break;
                        }
                    case nailStates.hasPicture:
                        {
                            if (col.name == "Bouquet")
                            {
                                if (gotFlowers)
                                {
                                    col.gameObject.SetActive(false);
                                    gameObject.GetComponent<SpriteRenderer>().sprite = nailSprite;
                                    myState = nailStates.hasBouquet;
                                    goodPicture.SetActive(true);
                                    goodPicture.transform.position = gameObject.transform.position;

                                    if (col.gameObject.activeSelf)
                                        col.SendMessage("RemoveBouquet");

                                    Debug.Log("swag");
                                }
                                else
                                {
                                    gameObject.GetComponent<SpriteRenderer>().sprite = nailUglyPicture;
                                    myState = nailStates.uglyPicture;
                                }
                            }
                            break;
                        }
                }
            }
            else
            {
                colName = col.name;
                onGoal = true;
                player.SendMessage("SetTargetPos", pathfindingPos);
            }
        }
    }

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "Dreamworld_Level01")
        {
            GameObject tempObject = GameObject.Find("MotherPicture");
            if (tempObject != null)
            {
                tempObject.SendMessage("SetGoalDistance", goalDistance);
                tempObject.SendMessage("SetGoalPathfindingPos", pathfindingPos);
            }
        }
    }
}
