using UnityEngine;
using System.Collections;


// !OBS! NPC class mÂste finnas


//Exempel fˆr hur detta anv‰nds i en Player klass
public class Player : MonoBehaviour {
    //Skapar ett nytt movement objek(l‰nka Movement scriptet till denna)
    public Pathfinding pathfinding;
    //Hur lÂngt ifrÂn kameran objektet ‰r (Till fˆr att positionen ska hamna r‰tt frÂn sk‰rm till v‰rlden)
    public float distance = 0f;
    public Animator anim;
    float interval = 0.2f;
    float nextTime = 0;

    private bool holdingItem = false;
    private Vector3 targetPosition = Vector3.zero;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(int scene)
    {
        if (scene != 0)
        {
            string newRoom = SharedVariables.NewRoom;
            GameObject[] mainCameras = GameObject.FindGameObjectsWithTag("MainCamera");
            foreach (GameObject camera in mainCameras)
            {
                camera.GetComponent<CameraMovement>().target = this.transform;
                camera.GetComponent<Camera>().enabled = false;
            }
            GameObject.Find(newRoom + "_Camera").GetComponent<Camera>().enabled = true;
            pathfinding.setGrid(GameObject.Find(newRoom + "_Background"));
            if (SharedVariables.OutFromDreamworld)
                transform.position = GameObject.Find(newRoom + "_DreamworldSpawn").GetComponent<Transform>().position;
            else
                transform.position = GameObject.Find(newRoom + "_Spawn").GetComponent<Transform>().position;


        }
    }


    // Update is called once per frame
    void Update() {
        Debug.Log(holdingItem.ToString());
        if (Time.time >= nextTime) {
            if (Input.GetMouseButton(0) && !holdingItem) {
                //s‰tter target som punkten man klickade pÅE
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //S‰tter r‰tt z position
                targetPosition.z = distance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //kollar ifall rayen tr‰ffar nÂgot
                if (Physics.Raycast(ray, out hit, 11.0f)) {
                    //letar specifikt efter tr‰ff mot golvet
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



