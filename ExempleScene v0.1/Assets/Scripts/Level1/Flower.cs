using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {

    public GameObject glove;
    public float goalDistance;
    public Vector3 pathfindingPos;
    public GameObject otherFlower;
    public Sprite invSprite;

    private GameObject player;
    private bool onGoal = false;
    private bool clicked = false;
    private bool mouseOutside = true;
    private bool scaled = false;

    enum States
    {
        SLEEPING,
        BOUQUET,
    };

    private States myState = States.SLEEPING;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= goalDistance) {
            clicked = false;
            //Dialougue: Säg att man behöver vantar för att plocka blommorna/Säg att blommorna är för taggiga för att plocka?
        }


        if (onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance) {
            if (scaled)
            {
                scaled = false;
                float tempX, tempY;
                tempX = gameObject.GetComponent<BoxCollider>().size.x;
                tempY = gameObject.GetComponent<BoxCollider>().size.y;
                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);

                gameObject.GetComponent<BoxCollider>().size = new Vector3(tempX / 0.7f, tempY / 0.7f, 0.2f);
            }

            Bouquet.bouquetInstance.SendMessage("AddFlower", gameObject);
            gameObject.transform.parent = Bouquet.bouquetInstance.gameObject.transform;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = Inventory.invInstance.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = invSprite;
            onGoal = false;
            glove.SendMessage("OnFlower", gameObject);
            gameObject.GetComponent<AudioSource>().Play();
            myState = States.BOUQUET;

            if (otherFlower != null)
                otherFlower.GetComponent<BoxCollider>().enabled = false;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (mouseOutside)
                clicked = false;

            onGoal = false;
        }
    }

    void OnTriggerStay(Collider col) {
        if (col.name == glove.name && !Input.GetMouseButton(0)) {
            if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance) {
                if (scaled)
                {
                    scaled = false;
                    float tempX, tempY;
                    tempX = gameObject.GetComponent<BoxCollider>().size.x;
                    tempY = gameObject.GetComponent<BoxCollider>().size.y;
                    gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);

                    gameObject.GetComponent<BoxCollider>().size = new Vector3(tempX / 0.7f, tempY / 0.7f, 0.2f);
                }
                Bouquet.bouquetInstance.SendMessage("AddFlower", gameObject);
                gameObject.transform.parent = Bouquet.bouquetInstance.gameObject.transform;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = Inventory.invInstance.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = invSprite;
                onGoal = false;
                glove.SendMessage("OnFlower", gameObject);
                gameObject.GetComponent<AudioSource>().Play();
                myState = States.BOUQUET;

                if (otherFlower != null)
                    otherFlower.GetComponent<BoxCollider>().enabled = false;
            } else {
                player.SendMessage("SetTargetPos", pathfindingPos);
                onGoal = true;
            }
        }
    }

    void Remove() {
        gameObject.SetActive(false);
    }

    void OnMouseUp() {
        player.SendMessage("SetTargetPos", pathfindingPos);
        clicked = true;
    }

    void OnMouseEnter() {
        mouseOutside = false;
        Debug.Log("enter flower: " + gameObject.name);
        if (myState == States.SLEEPING)
        {
            scaled = true;
            float tempX, tempY;
            tempX = gameObject.GetComponent<BoxCollider>().size.x;
            tempY = gameObject.GetComponent<BoxCollider>().size.y;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 1.3f, gameObject.transform.localScale.y * 1.3f, 1);

            gameObject.GetComponent<BoxCollider>().size = new Vector3(tempX * 0.7f, tempY * 0.7f, 0.2f);
        }
    }

    void OnMouseExit() {
        mouseOutside = true;
        Debug.Log("exit flower: " + gameObject.name);
        if (myState == States.SLEEPING)
        {
            scaled = false;
            float tempX, tempY;
            tempX = gameObject.GetComponent<BoxCollider>().size.x;
            tempY = gameObject.GetComponent<BoxCollider>().size.y;
            gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);

            gameObject.GetComponent<BoxCollider>().size = new Vector3(tempX / 0.7f, tempY / 0.7f, 0.2f);
        }
    }
}