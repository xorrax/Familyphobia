using UnityEngine;
using System.Collections;

public class Mower : MonoBehaviour {

    public GameObject player;
    public GameObject toothBrush;
    public Camera camera;

    private bool angry = true;
    private Vector3 completePos;

    void Start(){
        
    }


    void FixedUpdate(){
        if(camera.gameObject.activeSelf && angry){
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

            if(transform.position.y > -1.5f)
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        }
        
        if(!angry){
            Vector3.MoveTowards(transform.position, completePos, 1);
            if (Vector3.Distance(transform.position, completePos) < 0.5f){
                //Spela ljud för gräsklippning
            }
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.gameObject == toothBrush && !Input.GetMouseButton(0)){
            angry = false;
            //Byt mesh!
            //Säg att han kan klippa gräset runt trädet
        }
    }



}
