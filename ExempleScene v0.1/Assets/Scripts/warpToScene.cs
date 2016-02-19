using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class warpToScene : MonoBehaviour {
    public string nameOfScene;
    public void LoadScene(string level) {
        SceneManager.LoadScene(level);
    }

	// Use this for initialization
    void OnMouseDown() {
        LoadScene(nameOfScene);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
