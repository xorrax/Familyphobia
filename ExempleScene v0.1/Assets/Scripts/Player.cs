using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// !OBS! NPC class måste finnas


//Exempel för hur detta används i en Player klass
public class Player : MonoBehaviour {
    //Skapar ett nytt movement objek(länka Movement scriptet till denna)
    public Pathfinding pathfinding;
    //Hur långt ifrån kameran objektet är (Till för att positionen ska hamna rätt från skärm till världen)
    public float distance = 0f;
    public Animation animation;
    public Animator anim;
    float interval = 0.2f;
    float nextTime = 0;
	
    public static List<GameObject> dialogueObjects = new List<GameObject>();
	
    private bool interacted = false;
    private bool fishing = false;
    private GameObject interactedObject;
    private bool canMove = true;
    private Vector3 targetPosition = Vector3.zero;

    void Start() {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(int scene) {
        if (scene != 0) {
            string newRoom = SharedVariables.NewRoom;
            GameObject[] mainCameras = GameObject.FindGameObjectsWithTag("MainCamera");
            foreach (GameObject camera in mainCameras) {
                camera.GetComponent<CameraMovement>().target = GameObject.Find("Jack").GetComponent<Transform>();
                camera.GetComponent<Camera>().enabled = false;
            }
            GameObject loadingScreen = GameObject.Find("LoadingScreen");
            loadingScreen.transform.GetChild(0).gameObject.SetActive(false);
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
        if (Input.GetKeyDown("space")) {
            anim.Play("Fishing");
            anim.Play("Idle");

        }


        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime  > 1 && !anim.IsInTransition(0) && fishing)
        {
            anim.Play("Idle");
            fishing = false;
            canMove = true;
        }

        if (Time.time >= nextTime) {
            if (Input.GetMouseButton(0) && canMove) {
                //sätter target som punkten man klickade på
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Sätter rätt z position
                targetPosition.z = distance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //kollar ifall rayen träffar något
                if (Physics.Raycast(ray, out hit, 11.0f)) {
                    //letar specifikt efter träff mot golvet
                    if (hit.collider.tag == "floor" || hit.collider.tag == "warp") {
                        //Skickar positionen till target i movement
                        pathfinding.Target = targetPosition;
                    }

                    float tempDist = gameObject.GetComponent<BoxCollider>().bounds.size.x + hit.collider.bounds.size.x + 1;
                    if (Vector3.Distance(transform.position, hit.transform.position) <= tempDist) {

                        if (hit.collider.tag == "NPC" || hit.collider.tag == "Interactable") {
                            hit.collider.GetComponent<NPC>().self.interact();
                        }
                    } else {
                        interacted = true;
                        interactedObject = hit.collider.gameObject;
                    }
                }
                nextTime += interval;
            }
        }

        if (interacted) {
            float tempDist = gameObject.GetComponent<BoxCollider>().bounds.size.x + interactedObject.GetComponent<Collider>().bounds.size.x + 1;
            if (Vector3.Distance(transform.position, interactedObject.transform.position) <= tempDist) {

                if (interactedObject.tag == "NPC" || interactedObject.tag == "Interactable") {
                    interactedObject.GetComponent<NPC>().self.interact();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
            interacted = false;

        GC();
    }


    void CanWalk(bool value) {
        canMove = value;
    }

    void SetTargetPos(Vector3 pos) {
        pathfinding.Target = pos;
    }

    void FishAnimation(GameObject o)
    {
        canMove = false;
        anim.Play("Fishing");
        fishing = true;
    }

    void GC()
    {
        if(Time.frameCount % 30 == 0)
            System.GC.Collect();
    }
}






//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;


//// !OBS! NPC class måste finnas


////Exempel för hur detta används i en Player klass
//public class Player : MonoBehaviour {
//    //Skapar ett nytt movement objek(länka Movement scriptet till denna)
//    public Pathfinding pathfinding;
//    //Hur långt ifrån kameran objektet är (Till för att positionen ska hamna rätt från skärm till världen)
//    public float distance = 0f;
//    public Animator anim;
//    float interval = 0.2f;
//    float nextTime = 0;

//    private bool holdingItem = false;
//    private Vector3 targetPosition = Vector3.zero;

//    void Start() {
//        DontDestroyOnLoad(this.gameObject);
//    }

//    void OnLevelWasLoaded(int scene) {
//        if (scene != 0)
//        {
//            string newRoom = SharedVariables.NewRoom;
//            GameObject[] mainCameras = GameObject.FindGameObjectsWithTag("MainCamera");
//            foreach (GameObject camera in mainCameras) {
//                camera.GetComponent<CameraMovement>().target = GameObject.Find("Jack").GetComponent<Transform>();
//                camera.GetComponent<Camera>().enabled = false;
//            }
//            GameObject.Find(newRoom + "_Camera").GetComponent<Camera>().enabled = true;
//                pathfinding.setGrid(GameObject.Find(newRoom + "_Background"));
//            if (SharedVariables.OutFromDreamworld) 
//                transform.position = GameObject.Find(newRoom + "_DreamworldSpawn").GetComponent<Transform>().position;
//            else
//                transform.position = GameObject.Find(newRoom + "_Spawn").GetComponent<Transform>().position;
            
            
//        }
//    }

//    // Update is called once per frame
//    void Update() {
//        if (Input.GetKeyDown("space")) {
//            anim.Play("Fishing");
//            anim.Play("Idle");

//        }
//        if (Time.time >= nextTime) {
//            if (Input.GetMouseButton(0) && !holdingItem) {
//                //sätter target som punkten man klickade på
//                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                //Sätter rätt z position
//                targetPosition.z = distance;
//                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                RaycastHit hit;
//                //kollar ifall rayen träffar något
//                if (Physics.Raycast(ray, out hit, 11.0f)) {
//                    //letar specifikt efter träff mot golvet
//                    if (hit.collider.tag == "floor" || hit.collider.tag == "warp") {
//                        //Skickar positionen till target i movement
//                        pathfinding.Target = targetPosition;
//                    } else if (hit.collider.tag == "NPC") {
//                        hit.collider.GetComponent<NPC>().self.interact();
//                    }
//                }
//                nextTime += interval;
//            }
//        }
//    }


//    void HoldingItem(bool value) {
//        holdingItem = value;
//    }

//    void SetTargetPos(Vector3 pos) {
//        pathfinding.Target = pos;
//    }
//}



