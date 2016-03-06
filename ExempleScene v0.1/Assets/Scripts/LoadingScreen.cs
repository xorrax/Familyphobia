using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

    private string sceneToLoad;
    private GameObject player;
    AsyncOperation async;
    private float time;
    public float loadTime = 1.5f;

	void Start() {
        sceneToLoad = SharedVariables.NewScene;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("LoadLevel");
	}
	
	// Update is called once per frame
	void Update () {
        if (time > loadTime) {
            async.allowSceneActivation = true;
        }
        time += Time.deltaTime;
	}

    IEnumerator LoadLevel() {
        async = SceneManager.LoadSceneAsync(sceneToLoad);
        async.allowSceneActivation = false;
        while (!async.isDone) {

            yield return null;
        }
        player.SetActive(true);
    }
}
