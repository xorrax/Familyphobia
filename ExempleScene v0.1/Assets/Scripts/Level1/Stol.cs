using UnityEngine;
using System.Collections;

public class Stol : MonoBehaviour {

    public GameObject cupcake;
    public GameObject linda;
    public Sprite sprite;

    void Start(){
        cupcake = GameObject.Find("CupCake");
        linda = GameObject.Find("Linda");
    }

    void OnTriggerStay2D(Collider2D col){
        Debug.Log("col");
        if (col.name == cupcake.name && !Input.GetMouseButton(0))
        {
            linda.SendMessage("IsDistracted", true);
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            cupcake.SetActive(false);
        }
    }
}
