using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour{

	public string nextScene;
    public string nextRoom;

    public void NextScene(string level, string room) {
        SceneManager.LoadScene(level);
    }

    void Start() {
        SharedVariables.NewRoom = nextRoom;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentRoom = nextRoom;
        NextScene(nextScene, nextRoom);
    }
}
