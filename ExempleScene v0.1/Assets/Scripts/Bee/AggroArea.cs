using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AggroArea : MonoBehaviour {
    public Slider aggroBar;
    public Bi bi;

        void OnTriggerEnter(Collider col) {
        if (col.name == "Jack" && bi.myState == Bi.attackState.charging) {
            
            aggroBar.value++;
            Debug.Log(aggroBar.value);
        }
    }
}