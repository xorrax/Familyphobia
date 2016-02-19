using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    //hastigheten som objektet rör sig i
    public float speed = 2f;

    private Vector3 target;

    public Vector3 Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
            //Stoppar rutinen ifall den får ett nytt target
            StopCoroutine("Move");
            //startar upp den med det nya targetet
            StartCoroutine("Move", target);
        }
    }

    //Kod för en rutin
    IEnumerator Move(Vector3 target)
    {
        //kollar ifall objektet har kommit fram till sin target position
        while (transform.position != target)
        {
            //förflyttar positionen mot sitt target
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            yield return null;
        }
    }
}



