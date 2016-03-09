using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AggroBar : MonoBehaviour {
    
    public Slider aggroBar;
    public GameObject bee;
    private float targetValue;

	void Update () {
        aggroBar.transform.position =  new Vector3(bee.transform.position.x + 1.5f, bee.transform.position.y + 0.5f ,0);
        targetValue = Bi.aggro;
       
	}
}
