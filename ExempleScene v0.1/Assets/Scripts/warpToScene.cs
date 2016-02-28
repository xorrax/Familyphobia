using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class warpToScene : MonoBehaviour {
    public string nameOfScene;
    public string spawnRoom;
    public float newSceneScale = 0.5f;
    public float newSceneDepth = 1.4f;
    public float newSceneDepthOffset = 3.7f;
    public bool outFromDreamWorld;

    public void LoadScene(string level) {
        SharedVariables.NewRoom = spawnRoom;
        SharedVariables.OutFromDreamworld = outFromDreamWorld;
        Player player = GameObject.Find("Jack").GetComponent<Player>();
        player.gameObject.GetComponent<FakePerspective>().setStartScale(new Vector3(newSceneScale, newSceneScale, player.transform.localScale.z));
        player.gameObject.GetComponent<FakePerspective>().depth = newSceneDepth;
        player.gameObject.GetComponent<FakePerspective>().depthOffset = newSceneDepthOffset;
        SceneManager.LoadScene(level);
    }

    // Use this for initialization

    // Update is called once per frame
    void Update() {

    }
}