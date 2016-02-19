using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {

    public GameObject glove;
    public float goalDistance;
    public Vector3 pathfindingPos;

    private GameObject player;
    private bool onGoal = false;
    private bool clicked = false;
    private bool mouseOutside = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (clicked && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            clicked = false;
            //Dialougue: Säg att man behöver vantar för att plocka blommorna/Säg att blommorna är för taggiga för att plocka?
        }


        if(onGoal && Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
        {
            Bouquet.bouquetInstance.SendMessage("AddFlower", gameObject);
            gameObject.transform.parent = Bouquet.bouquetInstance.gameObject.transform;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = Inventory.invInstance.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            onGoal = false;
            glove.SendMessage("OnFlower", gameObject);
            gameObject.GetComponent<AudioSource>().Play();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(mouseOutside)
                clicked = false;

            onGoal = false;
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.name == glove.name && !Input.GetMouseButton(0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= goalDistance)
            {
                Bouquet.bouquetInstance.SendMessage("AddFlower", gameObject);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = Inventory.invInstance.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
                gameObject.transform.parent = Bouquet.bouquetInstance.gameObject.transform;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                player.SendMessage("SetTargetPos", pathfindingPos);
                onGoal = true;
            }
        }
    }

    void ChangeSprite(Sprite sprite){
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Remove(){
        gameObject.SetActive(false);
    }

    void OnMouseUp()
    {
        player.SendMessage("SetTargetPos", pathfindingPos);
        clicked = true;
    }

    void OnMouseEnter()
    {
        mouseOutside = false;
    }

    void OnMouseExit()
    {
        mouseOutside = true;
    }
}
