using UnityEngine;
using System.Collections;


// !OBS! NPC class m�ste finnas


//Exempel f�r hur detta anv�nds i en Player klass
public class Player : MonoBehaviour {
    //Skapar ett nytt movement objek(l�nka Movement scriptet till denna)
    public Pathfinding pathfinding;
    //Hur l�ngt ifr�n kameran objektet �r (Till f�r att positionen ska hamna r�tt fr�n sk�rm till v�rlden)
    public float distance = 0f;
    public Animator anim;
    float interval = 0.2f;
    float nextTime = 0;

    private bool holdingItem = false;
    private Vector3 targetPosition = Vector3.zero;

    void Start() {
        pathfinding = GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(holdingItem.ToString());
        if (Time.time >= nextTime) {
            if (Input.GetMouseButton(0) && !holdingItem) {
                //s�tter target som punkten man klickade p�E
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //S�tter r�tt z position
                targetPosition.z = distance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //kollar ifall rayen tr�ffar n�got
                if (Physics.Raycast(ray, out hit, 11.0f)) {
                    //letar specifikt efter tr�ff mot golvet
                    if (hit.collider.tag == "floor" || hit.collider.tag == "warp") {
                        //Skickar positionen till target i movement
                        pathfinding.Target = targetPosition;
                    } else if (hit.collider.tag == "NPC") {
                        hit.collider.GetComponent<NPC>().self.interact();
                    }
                }
                nextTime += interval;
            }
        }
    }


    void HoldingItem(bool value) {
        holdingItem = value;
    }

    void SetTargetPos(Vector3 pos) {
        pathfinding.Target = pos;
    }
}



