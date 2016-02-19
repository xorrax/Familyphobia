using UnityEngine;
using System.Collections;

public class Linda : MonoBehaviour {

    private bool isDistraced = false;
    private GameObject key;

    void IsDistracted(bool value){
        isDistraced = value;
        key = GameObject.Find("Key");
        key.SendMessage("LindaDistracted", value);
    }

    void HasWorm(){
        //Linda säger att hon inte gillar mask och bara söta saker?
    }
}
