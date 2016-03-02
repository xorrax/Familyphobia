using UnityEngine;
using System.Collections;

public class Linda : MonoBehaviour {

    private bool isDistraced = false;
    public GameObject key;

    void IsDistracted(bool value){
        isDistraced = value;
        key.SendMessage("LindaDistracted", value);
    }

    void HasWorm(){
        //Linda säger att hon inte gillar mask och bara söta saker?
    }
}
