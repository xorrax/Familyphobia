using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    //hastigheten som objektet rör sig i
    public float speed = 2f;
    private Animator anim;
    private Pathfinding pathfinding;
    Vector3 lastPosition;

    private Vector3 target;

    void Start() {
        anim = GetComponent<Animator>();
        pathfinding = GetComponent<Pathfinding>();
    }

    public Vector3 Target {
        get { 
            return target; 
        }
        set {
            target = value;
            //Stoppar rutinen ifall den får ett nytt target
            StopCoroutine("Move");
            //startar upp den med det nya targetet
            StartCoroutine("Move", target);
            anim.SetBool("Walking", true);

        }
    }

    //Kod för en rutin
    IEnumerator Move(Vector3 target) {
        //kollar ifall objektet har kommit fram till sin target position
        
        while (transform.position != target)
        {
            anim.SetBool("Walking", true);
            //förflyttar positionen mot sitt target
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            Vector3 direction = target - transform.position;
            direction.Normalize();
            anim.SetFloat("hSpeed", direction.x);
            anim.SetFloat("vSpeed", direction.y);
            if (pathfinding.getTargetNodePosition() == transform.position) {
                anim.SetBool("Walking", false);
                anim.SetFloat("hSpeed", 0);
                anim.SetFloat("vSpeed", 0);
            }

            lastPosition = transform.position;
            yield return null;
        }
        anim.SetBool("Walking", false);
        anim.SetFloat("hSpeed", 0);
        anim.SetFloat("vSpeed", 0);
    }

    public void setSpeed(float newSpeed) {
        speed = newSpeed;
    }

    public float getSpeed() {
        return speed;
    }
}
