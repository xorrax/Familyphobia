using UnityEngine;
using System.Collections;

public class Glove : MonoBehaviour {
    public GameObject blueFlower;
    public GameObject redFlower;
    public GameObject brownFlower;
    public GameObject orangeFlower;
    public float goalDistance;
    public Vector3 pathfindingPos;

    private Color mouseOverColor = Color.green;
    private Color originalColor;
    private bool myDragging = false;
    private bool otherDragging = false;
    private bool canCombine = false;
    private bool hasCombined = false;
    private bool clicked = false;
    private bool mouseOutside = true;
    private Vector2 myPos = new Vector2();
    private GameObject player;
    private GameObject otherComboObject;
    private int brokenValue = 0;
    private enum invState {
        DRAGGING,  //..
        INVENTORY, //I inventory
        SLEEPING, //Innan man gjort något med objektet
        COMBINATION, //när man kombinerar två objekt
    };
    private invState otherState = invState.SLEEPING;
    private invState myState = invState.SLEEPING;

    private float myDistance;

    void Start() {
        originalColor = this.gameObject.GetComponent<Renderer>().material.color;

        if (gameObject.tag != "CombinationTrigger")
            canCombine = true;

        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update() {

        this.gameObject.GetComponent<Rigidbody2D>().WakeUp();

        if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance && clicked) {
            clicked = false;
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
            gameObject.GetComponent<AudioSource>().Play();
        }

        if (Input.GetMouseButtonDown(0) && mouseOutside)
            clicked = false;
    }

    void FixedUpdate() {
        if (myDragging && myState != invState.SLEEPING) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(myDistance);
            transform.position = rayPoint;
        }
    }

    void OnFlower(GameObject col) {
        myState = invState.COMBINATION;
        brokenValue++;
        if (brokenValue >= 4) {
            gameObject.SetActive(false);
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
        }
    }


    void OnTriggerExit(Collider col) {

        if (col.name == "Inventory") {
            Inventory.invInstance.SendMessage("RemoveItem", this.gameObject);
            Inventory.invInstance.SendMessage("SetPositions");
            myState = invState.DRAGGING;
        }
    }

    void OtherState(invState value) {
        otherState = value;
    }
    void OtherDragging(bool dragging) {
        otherDragging = dragging;
    }

    void OtherCombObject(GameObject otherObject) {
        otherComboObject = otherObject;
    }
    void SetPosition(Vector2 pos) {
        myPos = pos;
    }

    void CanCombine(bool value) {
        canCombine = value;
    }

    void OnMouseEnter() {
        this.gameObject.GetComponent<Renderer>().material.color = mouseOverColor;
        mouseOutside = false;
    }

    void OnMouseExit() {
        this.gameObject.GetComponent<Renderer>().material.color = originalColor;
        mouseOutside = true;
    }

    void OnMouseDown() {
        myDistance = Vector2.Distance(transform.position, Camera.main.transform.position);
        myDragging = true;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
        player.SendMessage("CanWalk", false);
    }

    void OnMouseUp() {
        myDragging = false;

        if (myState == invState.SLEEPING) {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= goalDistance) {
                Inventory.invInstance.SendMessage("AddItem", this.gameObject);
                myState = invState.INVENTORY;
                gameObject.GetComponent<AudioSource>().Play();
            } else {
                clicked = true;
                player.SendMessage("SetTargetPos", pathfindingPos);
            }
        } else if (myState == invState.INVENTORY) {
            this.gameObject.transform.position = myPos;
            Inventory.invInstance.SendMessage("SetPositions");
        } else if (myState != invState.INVENTORY && myState != invState.COMBINATION) {
            StartCoroutine(wait());
            Inventory.invInstance.SendMessage("RemoveItem", gameObject);
            Inventory.invInstance.SendMessage("AddItem", this.gameObject);
            myState = invState.INVENTORY;
        }
        player.SendMessage("CanWalk", true);
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
    }

    IEnumerator wait() {
        yield return 0;
    }

    IEnumerator endOfFrame() {
        yield return new WaitForEndOfFrame();
    }
}
