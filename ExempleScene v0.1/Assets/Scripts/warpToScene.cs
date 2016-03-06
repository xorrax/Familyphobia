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
    private MobileToggle mobileToggle;

    public void LoadScene(string level) {
        mobileToggle = GameObject.Find("Mobile").GetComponent<MobileToggle>();

        mobileToggle.setToggle(false);
        SharedVariables.NewRoom = spawnRoom;
        SharedVariables.NewScene = level;
        SharedVariables.OutFromDreamworld = outFromDreamWorld;
        Player player = GameObject.Find("Jack").GetComponent<Player>();
        player.gameObject.GetComponent<FakePerspective>().setStartScale(new Vector3(newSceneScale, newSceneScale, player.transform.localScale.z));
        player.gameObject.GetComponent<FakePerspective>().depth = newSceneDepth;
        player.gameObject.GetComponent<FakePerspective>().depthOffset = newSceneDepthOffset;
        SceneManager.LoadScene("LoadingScreen");
    }

}