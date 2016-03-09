using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bi : MonoBehaviour {
    
    /* Kortare men oftare charge */
    /* Fixade aggro bar */
    /*Aggro radius, växer med aggro*/
    private GameObject aggroElips;
    private GameObject aggroFill;
    private GameObject aggroZone;
    private GameObject aggroRange;

    private AttackRange attackRange;


    private float aggroLoss;
    private float aggroLossTick;

    private string Life = "5";
    public Text lifeText;

    public Slider slider;
    private GameObject player;

    public int softObstacleDurability;
    private int colDurability = 0;

    private Vector3 startPos;

    private Vector3 extraLength;
    private Vector3 dir = Vector3.zero;
    private Vector3 playerNormalizedDirection = Vector3.zero;
    private Vector3 tempPos;
    private Vector3 targetPos;
    private Vector3 lastPos;
    private float acc = 0;

    public float speed;
    public float chargeUpTime = 3;

    private bool collision;

    static public int aggro = 0;

    private float restTime = 0f;
    private float chargeTime = 0f;
    public float distance;

    private float myDistance;

    public attackState myState;

    private Color chargeColor;
    private Color defaultColor;

    private GameObject colObject;

    public enum attackState{
        charging,
        waiting,
        resting,
        stuck,
    }

	void Start () {
        

        defaultColor = gameObject.GetComponent<SpriteRenderer>().color;
        
        chargeColor  = Color.red;
        startPos     = transform.position;
        myState      = attackState.resting;
        lastPos      = transform.position;

        player     = GameObject.Find("Jack");

        aggroElips = GameObject.Find("AggroElips");
        aggroFill  = GameObject.Find("elipsFill");

        aggroZone  = GameObject.Find("AggroZone"); 
        aggroRange = GameObject.Find("AggroRange");

        attackRange = aggroRange.GetComponent<AttackRange>();



    }

    void FixedUpdate(){
        if (slider.value == 0) {
            aggroLoss = 3;
            
        }
        else if (slider.value == 1) {
            aggroLoss = 2.33f;
            speed = 6 * 1.3f;
        }
        else if (slider.value == 2) {
            aggroLoss = 2;
            speed = 7.8f * 1.3f;
        }
        else if (slider.value == 3) {
            aggroLoss = 1.6f;
            speed = 10.14f * 1.3f;
        }
        else if (slider.value == 4) {
            aggroLoss = 1.33f;
            speed = 13.182f * 1.3f;
        }
        else if (slider.value == 5) {
            aggroLoss = 1;
            speed = 17.1366f * 1.3f;
        }

        if (!attackRange.inRange) {
            myState = attackState.resting;
            attackRange.transform.position = transform.position;
            aggroLossTick += Time.deltaTime;
            if(aggroLossTick >= aggroLoss) {
                slider.value--;
                aggroLossTick = 0;
            }
            Debug.Log(attackRange.inRange);
        }
       
       
        dir = player.transform.position - gameObject.transform.position;
        //Vector3 jackPosition = targetPos;
        //Sätter rätt z position
        //jackPosition.z = distance;

        Debug.DrawRay(transform.position, targetPos, Color.green);
        Debug.DrawRay(transform.position, playerNormalizedDirection, Color.blue);
        switch (myState){
            
            case attackState.charging:{
                    aggroElips.transform.position = transform.position;
                    aggroRange.transform.position = transform.position;
                    //acc += Time.deltaTime;
                    //gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * acc);
                    gameObject.transform.position += playerNormalizedDirection * speed * Time.deltaTime;
                    
                    myDistance = Vector3.Distance(Vector3.zero, targetPos);
                    lastPos = transform.position;

                    break;
                }

            case attackState.resting:{
                    if (!attackRange.inRange) {
                        restTime = 0;
                    }
                    if (restTime == 0) {
                        aggroZone.transform.position = transform.position;
                        aggroElips.transform.position = transform.position;
                        gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
                    }
                    restTime += Time.deltaTime;
                    acc       = 0;

                    aggroFill.transform.localScale = new Vector3(restTime , restTime , 0); // floaten måstwe ändras ifall charge time ändras
                    if (restTime >= chargeUpTime) {
                        myState  = attackState.charging;
                        restTime = 0;
                        playerNormalizedDirection = (player.transform.position - gameObject.transform.position).normalized;
                        targetPos = (player.transform.position - gameObject.transform.position).normalized * distance;
                    }
                    else if (restTime >= chargeUpTime - 0.5f) {
                        gameObject.GetComponent<SpriteRenderer>().color = chargeColor;
                        //aggro++;
                       
                    }
                        

                    break;
                }

            case attackState.stuck: {
                    
                    break;
                }

            case attackState.waiting: {

                    gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
                    Vector3 rayDirection = player.transform.position - transform.position;

                    RaycastHit hit;
                    rayDirection.Normalize();
                    Ray ray = new Ray(transform.position + rayDirection, rayDirection);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity)){

                        Debug.Log(hit.collider.name + ", " + colObject.name);
                    }
                    myState = attackState.resting;
                    break;
                }
        }

    }

    //void OnTriggerStay(Collider col){
    //    Debug.Log("in object: " + col.name);
    //}

    void OnTriggerExit(Collider col) {
        if (col.name == "AggroZone") {
            Debug.Log("distance meet");
            aggroFill.transform.localScale = new Vector3(0, 0, 0);
            myState = attackState.resting;
            myDistance = 0;
            lastPos = transform.position;
            aggroZone.transform.position = transform.position;
            acc = 0;
        }
    }

    void OnTriggerEnter(Collider col){
   
        colObject = col.gameObject;

        //tempPos = player.transform.position;
        if (col.name == player.name) {
            //player.SendMessage("Reset");
            Reset();
        }
        else if (col.tag == "SoftObstacle") {
            Sprite temp = new Sprite();
            temp = col.GetComponent<SoftObstacle>().shrekt;
            col.GetComponent<SpriteRenderer>().sprite = temp;

            myState = attackState.waiting;
            restTime = 0;
            chargeTime = 0;
            dir.Normalize();
            gameObject.transform.position -= dir * 0.5f;

        }
        else if (col.tag == "SolidObstacle") {

            myState = attackState.waiting;
            restTime = 0;
            chargeTime = 0;
            dir.Normalize();
            gameObject.transform.position -= dir * 0.5f;
        }
        else if (col.tag == "Korkek") {
            if(slider.value < slider.maxValue) {
                myState = attackState.waiting;
                restTime = 0;
                chargeTime = 0;
                dir.Normalize();
                gameObject.transform.position -= dir * 0.5f;
            }
            else if(slider.value == slider.maxValue) {
                myState = attackState.stuck;

            }
        }

           
        
        else if (col.tag == "Balloon"){
            col.SendMessage("RemoveObstacleAggro");
        }
    }

   
    void Reset(){
        player.transform.position = new Vector3(56.53f, -0.131f, 0);
        aggroFill.transform.localScale = new Vector3(0, 0, 0);
        aggroRange.transform.position = transform.position;
        slider.value = slider.minValue;
        gameObject.transform.position = startPos;
        myState = attackState.resting;
        restTime = 0f;
        chargeTime = 0f;
        lastPos = startPos;
        myDistance = 0;
        aggroZone.transform.position = transform.position;
        acc = 0;
        speed = 6;
    }

    void GetDurability(int value){
        colDurability = value;
    }
}
