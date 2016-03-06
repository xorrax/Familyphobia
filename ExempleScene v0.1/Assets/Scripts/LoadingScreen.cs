using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

    private string sceneToLoad;
    private GameObject player;
    AsyncOperation async;

	void OnLevelWasLoaded () {
        sceneToLoad = SharedVariables.NewScene;
        player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        StartCoroutine("LoadLevel");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator LoadLevel() {
        async = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!async.isDone) {
            player.SetActive(true);
        }
        yield return null;
    }
}
