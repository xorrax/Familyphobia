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

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= goalDistance) {
            clicked = false;
            //Dialougue: Säg att man behöver vantar för att plocka blommorna/Säg att blommorna är för taggiga för att plocka?
        }


        if (onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance) {
            Bouquet.bouquetInstance.SendMessage("AddFlower", gameObject);
            gameObject.transform.parent = Bouquet.bouquetInstance.gameObject.transform;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = Inventory.invInstance.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = invSprite;
            onGoal = false;
            glove.SendMessage("OnFlower", gameObject);
            gameObject.GetComponent<AudioSource>().Play();

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
                Bouquet.bouquetInstance.SendMessage("AddFlower", gameObject);
                gameObject.transform.parent = Bouquet.bouquetInstance.gameObject.transform;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = Inventory.invInstance.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = invSprite;
                onGoal = false;
                glove.SendMessage("OnFlower", gameObject);
                gameObject.GetComponent<AudioSource>().Play();

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
    }

    void OnMouseExit() {
        mouseOutside = true;
    }
}